using UnityEngine;

public class PanzerfaustProjectile : MonoBehaviour
{
    private float _range;
    private int _damage;
    private PlayerController _owner;
    private Rigidbody2D rb;

    [SerializeField] private ShockWave _shockWave;
    [SerializeField] private float _explosionRadius = 1.5f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private float _boostDistance = 5f;
    [SerializeField] private float _maxSpeed = 20f;
    [SerializeField] private AnimationCurve _accelerationCurve;
    [SerializeField] private LayerMask obstacleLayer;

    private Vector2 _moveDir;
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
            if (_shockWave != null)
            {
                _shockWave.gameObject.SetActive(true);
                _shockWave?.CallShockWave(1,0.05f);
            }
            float t = Mathf.InverseLerp(_boostDistance, _range, dist); // Normalize 0 ~ 1
            float speedMultiplier = _accelerationCurve.Evaluate(t);
            float currentSpeed = _maxSpeed * speedMultiplier;
            rb.velocity = _moveDir * currentSpeed;
        }
    }

    public void Init(int damage, float speed, float range, PlayerController owner, Vector2 direction)
    {
        
        _damage = damage;
        _range = range;
        _owner = owner;
        _startPos = transform.position;
        _isExploded = false;

        _moveDir = direction.normalized;
        rb.velocity = _moveDir * speed;

        CancelInvoke();
        Invoke("CheckRangeAndExplode", 0.2f);
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
        Invoke("CheckRangeAndExplode", 0.05f);
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

        int colLayer = collision.gameObject.layer;
        if (colLayer == LayerMask.NameToLayer("Wall") ||
            colLayer == LayerMask.NameToLayer("AttackObj") ||
            colLayer == LayerMask.NameToLayer("Interact") ||
            colLayer == LayerMask.NameToLayer("Default"))
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
