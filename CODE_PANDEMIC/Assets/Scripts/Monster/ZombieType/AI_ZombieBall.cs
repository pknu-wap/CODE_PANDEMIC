using UnityEngine;

public class AI_ZombieBall : AI_Controller
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private GameObject explosionEffectPrefab;
    public float summonRadius = 1.5f;
    public float rushDuration = 5f;
    public float explosionRadius = 2.5f;
    public float explosionDamageMultiplier = 1.1f;

    public LayerMask TargetLayer; // 추가

    private AI_ZombieBall_RushSkill _rushSkill;
    private AI_ZombieBall_ExplosionSkill _explosionSkill;

    public override ISkillBehavior Skill
    {
        get
        {
            return _rushSkill;
        }
    }

    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player"); // 초기화
        base.Awake();
        _rushSkill = new AI_ZombieBall_RushSkill();
        _explosionSkill = new AI_ZombieBall_ExplosionSkill();

        _rushSkill.RushDuration = rushDuration;
        _rushSkill.SetSettings(null, TargetLayer, this);

        _explosionSkill.explosionEffectPrefab = explosionEffectPrefab;
        _explosionSkill.explosionRadius = explosionRadius;
        _explosionSkill.explosionDamageMultiplier = explosionDamageMultiplier;
        _explosionSkill.summonRadius = summonRadius;
        _explosionSkill.zombiePrefab = zombiePrefab;
        _explosionSkill.SetSettings(null, TargetLayer, this);
    }

    protected override void Start()
    {
        base.Start();
        if (!Init())
        {
            enabled = false;
            return;
        }
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (!_rushSkill.IsRushing)
        {
            ForceDetectTarget(_detection.Player);
            _rushSkill.StartSkill(this, null);
        }
        if (Health <= 0)
        {
            _explosionSkill.StartSkill(this, null);
        }
    }

    protected override void Dielogic()
    {
        if (_isDead) return;
        _isDead = true;
        Managers.Game.AddZombieKillCount();
        _combat.StopSkill();
        _rb.velocity = Vector2.zero;
        _movement.StopMoving();
        _explosionSkill.StartSkill(this, null); // 폭발 로직 호출
        ChangeState<AI_StateDie>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;
        LayerMask playerMask = LayerMask.GetMask("Player");
        LayerMask wallMask = LayerMask.GetMask("Wall");

        if (((1 << otherLayer) & playerMask) != 0 || ((1 << otherLayer) & wallMask) != 0 && !_isDead)
        {
            _explosionSkill.StartSkill(this, null);
        }
    }
    public override bool IsPlayerInSkillRange()
    {
        return false;
    }
}
