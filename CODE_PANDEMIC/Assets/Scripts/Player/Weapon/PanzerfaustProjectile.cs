using UnityEngine;

public class PanzerfaustProjectile : MonoBehaviour
{
    private float _speed;
    private float _range;
    private int _damage;
    private PlayerController _owner;
    private Rigidbody2D rb;

    [SerializeField] private float _explosionRadius = 2.5f; // 폭발 범위
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private GameObject _explosionEffect;

    private Vector3 _startPos;
    private bool _isExploded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(int damage, float speed, float range, PlayerController owner, Vector2 direction)
    {
        _damage = damage;
        _speed = speed;
        _range = range;
        _owner = owner;
        _startPos = transform.position;
        _isExploded = false;

        rb.velocity = direction.normalized * _speed;
        CancelInvoke();
        Invoke("CheckRangeAndExplode", 0.05f); // 반복적으로 사거리 체크
    }

    // 일정 거리 초과 체크용
    private void CheckRangeAndExplode()
    {
        if (_isExploded) return;

        float dist = Vector3.Distance(_startPos, transform.position);
        if (dist >= _range)
        {
            Explode();
            return;
        }
        Invoke("CheckRangeAndExplode", 0.02f); // 계속 체크
    }

    private void Explode()
    {
        if (_isExploded) return;
        _isExploded = true;

        // 폭발 이펙트
        if (_explosionEffect != null)
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);

        // 범위 내 적 탐지 및 데미지
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayer);
        foreach (var enemyCol in enemies)
        {
            AI_Base enemy = enemyCol.GetComponent<AI_Base>();
            if (enemy != null)
                enemy.TakeDamage(_damage);
        }

        // 상태 초기화 및 풀로 반환
        _damage = 0;
        _owner = null;
        rb.velocity = Vector2.zero;
        BulletPool.Instance.ReturnBullet(gameObject);
    }

    // 벽(지형) 충돌 시 폭발
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isExploded) return;
        // Enemy 레이어가 아니라면(벽 등), 폭발
        if (((1 << collision.gameObject.layer) & _enemyLayer) == 0)
        {
            Explode();
        }
    }

    // 적 트리거 충돌 시도 폭발 (옵션)
    private void OnTriggerEnter2D(Collider2D other)
    {
        AI_Base enemy = other.GetComponent<AI_Base>();
        if (enemy != null)
        {
            Explode();
        }
    }
}
