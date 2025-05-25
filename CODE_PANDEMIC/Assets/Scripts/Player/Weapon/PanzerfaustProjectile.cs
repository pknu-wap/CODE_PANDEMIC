using UnityEngine;

public class PanzerfaustProjectile : MonoBehaviour
{
    private float _speed;
    private float _range;
    private int _damage;
    private PlayerController _owner;
    private Rigidbody2D rb;

    [SerializeField] private float _explosionRadius = 2.5f;// 폭발 범위
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private float _boostDistance = 5f;    // 가ㅏ속 시작 범위
    [SerializeField] private float _acceleration = 30f;
    [SerializeField] private LayerMask obstacleLayer;


    private float _currentSpeed;                            // 현재 속도
    private Vector2 _moveDir;                               // 비행 방향
    private Vector3 _startPos;
    private bool _isExploded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isExploded) return;

        float dist = Vector3.Distance(_startPos, transform.position);
        if (dist >= _boostDistance)
        {
            _currentSpeed += _acceleration * Time.deltaTime;
            rb.velocity = _moveDir * _currentSpeed;
        }
    }


    public void Init(int damage, float speed, float range, PlayerController owner, Vector2 direction)
    {
        _damage = damage;
        _speed = speed*2f;
        _currentSpeed = speed;
        _range = range;
        _owner = owner;
        _startPos = transform.position;
        _isExploded = false;

        _moveDir = direction.normalized;
        rb.velocity = _moveDir * _currentSpeed;

        CancelInvoke();
        Invoke("CheckRangeAndExplode", 0.05f);
    }


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

        if (_explosionEffect != null)
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayer);
        foreach (var enemyCol in enemies)
        {
            AI_Base enemy = enemyCol.GetComponent<AI_Base>();
            if (enemy != null)
                enemy.TakeDamage(_damage);
        }

        _damage = 0;
        _owner = null;
        rb.velocity = Vector2.zero;
        PanzerfaustPool.Instance.ReturnRocket(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isExploded) return;

        int wallLayer = LayerMask.NameToLayer("Wall");           // 3
        int attackObjLayer = LayerMask.NameToLayer("AttackObj"); // 13
        int interactLayer = LayerMask.NameToLayer("Interact");   // 8
        int defaultLayer = LayerMask.NameToLayer("Default");     // 0

        int colLayer = collision.gameObject.layer;
        if (colLayer == wallLayer || colLayer == attackObjLayer || colLayer == interactLayer || colLayer == defaultLayer)
        {
            Explode();
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        AI_Base enemy = other.GetComponent<AI_Base>();
        if (enemy != null)
        {
            Explode();
        }
    }



}
