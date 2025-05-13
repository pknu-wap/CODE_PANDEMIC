using UnityEngine;

public class AI_ZombieBall : AI_Controller
{
    public GameObject zombiePrefab;
    public int maxSummonedZombies = 3;
    public float summonCooldown = 8f;
    public float summonRadius = 1.5f;
    public float rushSpeed = 12f;
    public float rushDuration = 2f;
    public float rushCooldown = 5f;

    private float _lastSummonTime = -Mathf.Infinity;
    private float _lastRushTime = -Mathf.Infinity;
    private float _rushEndTime = -Mathf.Infinity;

    private bool _isRushing = false;
    private Vector2 _rushDirection;

    protected override void Start()
    {
        base.Start();
    //     if (_monsterData == null)
    // {
    //     _monsterData = new MonsterData();
    //     _monsterData.NameID = "ZombieBall";
    //     _monsterData.Hp = 300;
    //     _monsterData.AttackDelay = 0;
    //     _monsterData.DetectionRange = 0;
    //     _monsterData.DetectionAngle = 0;
    //     _monsterData.MoveSpeed = 0.01f;
    //     _monsterData.AttackRange = 0;
    //     _monsterData.AttackDamage = 50;
    // }       
        ChangeState(new AI_StateIdle(this));
    }

    protected void Update()
    {
        TrySummon();

        if (_isRushing)
        {
            _rb.velocity = _rushDirection * rushSpeed;

            if (Time.time >= _rushEndTime)
            {
                _isRushing = false;
                _rb.velocity = Vector2.zero;
            }
        }
    }

    private void TrySummon()
    {
        if (Time.time < _lastSummonTime + summonCooldown || _player == null)
            return;

        int count = 0;
        for (int i = 0; i < maxSummonedZombies; i++)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * summonRadius;
            GameObject zombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

            AI_Controller summonZombie = zombie.GetComponent<AI_PatientZombie>();
            if (summonZombie != null)
            {
                summonZombie.ForceDetectTarget(_player);
            }

            count++;
        }

        if (count > 0)
        {
            _lastSummonTime = Time.time;
        }
    }

    public void TriggerRush(Vector2 direction)
    {
        if (_isRushing || Time.time < _lastRushTime + rushCooldown)
            return;

        _rushDirection = direction.normalized;
        _isRushing = true;
        _rushEndTime = Time.time + rushDuration;
        _lastRushTime = Time.time;
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (Health <= 0)
        {
            TrySummon();
        }
    }

    public override void ForceDetectTarget(Transform player)
    {
        base.ForceDetectTarget(player);

        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            TriggerRush(direction);
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
        }
    }
}
