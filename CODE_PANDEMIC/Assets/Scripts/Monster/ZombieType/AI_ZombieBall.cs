using UnityEngine;

public class AI_ZombieBall : AI_Controller
{
    public GameObject zombiePrefab;
    public GameObject explosionEffectPrefab;
    public float summonRadius = 1.5f;
    public float rushDuration = 5f;
    public float explosionRadius = 2.5f;
    public float explosionDamageMultiplier = 1.1f;

    private float _rushEndTime = -Mathf.Infinity;

    private bool _isRushing = false;
    private Vector2 _rushDirection;

    protected override void Start()
    {
    
    if (_monsterData == null)
    {
        _monsterData = new MonsterData();
        _monsterData.NameID = "ZombieBall";
        _monsterData.Hp = 300;
        _monsterData.AttackDelay = 0;
        _monsterData.DetectionRange = 0;
        _monsterData.DetectionAngle = 0;
        _monsterData.MoveSpeed = 7f;
        _monsterData.AttackRange = 0;
        _monsterData.AttackDamage = 50;
    }      
        base.Start();   
        ChangeState(new AI_StateIdle(this));
    }

    protected void Update()
    {

        if (!_isRushing && Input.GetKeyDown(KeyCode.R) && _player != null)
    {
        Debug.Log("Rush triggered");
        Vector2 dir = (_player.transform.position - transform.position).normalized;
        TriggerRush(dir);
    }
        if (_isRushing)
        {
            _rb.velocity = _rushDirection * MoveSpeed;

            if (Time.time >= _rushEndTime)
            {
                Explosion();
            }
        }
    }

    private void Explosion()
    {
        _isRushing = false;
        _rb.velocity = Vector2.zero;

        if (explosionEffectPrefab)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, LayerMask.GetMask("Player"));
        int damage = Mathf.RoundToInt(Damage * explosionDamageMultiplier);

        foreach (var target in targets)
        {
            if (target.TryGetComponent<PlayerStatus>(out var playerStatus))
            {
                playerStatus.OnDamaged(gameObject, damage);
            }
        }
        gameObject.SetActive(false);
        TrySummon();
    }

    private void TrySummon()
    {
        if (_player == null)
            return;
        int summonCount = Random.Range(3,6);
        for (int i = 0; i < summonCount; i++)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * summonRadius;
            GameObject zombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

            AI_Controller summonZombie = zombie.GetComponent<AI_PatientZombie>();
            if (summonZombie != null)
            {
                summonZombie.ForceDetectTarget(_player);
            }
        }
    }

    public void TriggerRush(Vector2 direction)
    {
        if (_isRushing) return;

        _rushDirection = direction.normalized;
        _isRushing = true;
        _rushEndTime = Time.time + rushDuration;
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (Health <= 0)
        {
            TrySummon();
            Explosion();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            PlayerController shooter = bullet.GetOwner();
            if (shooter != null)
            {
                Vector2 dir = (shooter.transform.position - transform.position).normalized;
                TriggerRush(dir);
            }
            return;
        }

        int otherLayer = other.gameObject.layer;
        LayerMask playerMask = LayerMask.GetMask("Player");
        LayerMask wallMask = LayerMask.GetMask("Wall");

        if (((1 << otherLayer) & playerMask) != 0 || ((1 << otherLayer) & wallMask) != 0)
        {
            Explosion();
        }
    }

}
