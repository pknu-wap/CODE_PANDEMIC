using UnityEngine;

public class AI_HospitalBoss : AI_BossController
{
    public LayerMask TargetLayer;
    public bool IsBerserk { get; private set; }

    public GameObject _syringePrefab;
    public float SyringeSpeed;
    public float ThrowCooldown = 10f;
    public float SyringeRange = 10f;
    public float SkillChargeDelay = 0.5f;

    public float SweepCooldown = 15f;
    public int SweepCount = 10;
    public float SweepInterval = 0.25f;
    public float SweepRange = 4f;
    public float SweepAngle = 120f;

    public float DashCooldown = 20f;
    public float DashRange = 12f;
    public float DashDuration = 0.8f;
    public float DashWidth = 5f;
    public float DashSpeed = 20f;

    private float _lastSkillTime = -Mathf.Infinity;
    private float _skillDelay = 3f;
    public AI_ThrowVisualizer _syringeVisualizer;
    public override float AiDamage => _monsterData.AttackDamage;
    public int MaxHealth => _monsterData.Hp;

    private AI_BossThrow _throwSkill = new AI_BossThrow();
    private AI_BossSweep _sweepSkill = new AI_BossSweep();
    private AI_BossDash _dashSkill = new AI_BossDash();

    private float _lastThrowTime = -Mathf.Infinity;
    private float _lastSweepTime = -Mathf.Infinity;
    private float _lastDashTime = -Mathf.Infinity;

    public override ISkillBehavior Skill => null;

    protected override void Start()
    {
        ChangeState(new AI_BossIdle(this));
        _throwSkill.SetController(this);
        _sweepSkill.SetController(this);
        _dashSkill.SetController(this);
        if (_monsterData == null)
        {
       _monsterData = new MonsterData();
       _monsterData.NameID = "HospitalBoss";
       _monsterData.Hp = 1000;
       _monsterData.AttackDelay = 5.0f;
       _monsterData.DetectionRange = 7.5f;
       _monsterData.DetectionAngle = 360;
       _monsterData.MoveSpeed = 3.5f;
       _monsterData.AttackRange = 2f;
       _monsterData.AttackDamage = 20;
    }
    EnterBerserkMode();
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
        if (!IsBerserk && Health <= MaxHealth * 0.5f)
        {
            EnterBerserkMode();
        }
        if (Health <= 0f && _currentState is not AI_BossDie)
            ChangeState(new AI_BossDie(this));
    }
    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();
    }
    private void EnterBerserkMode()
    {
        IsBerserk = true;
    }

   public override void TryUseSkill(System.Action onSkillComplete)
{
    if (_player == null)
    {
        onSkillComplete?.Invoke();
        return;
    }
    float now = Time.time;

    if (now < _lastSkillTime + _skillDelay)
    {
        onSkillComplete?.Invoke();
        return;
    }

    float distance = Vector2.Distance(transform.position, _player.position);

    if (IsBerserk && now >= _lastDashTime + DashCooldown)
    {
        _lastDashTime = now;
        _isUsingSkill = true;
        _dashSkill.StartSkill(this, onSkillComplete);
        _lastSkillTime = now;
        _isUsingSkill = false;

    }
    else if (distance >= 4f && now >= _lastThrowTime + ThrowCooldown)
    {
        _lastThrowTime = now;
        _isUsingSkill = true;
        _throwSkill.StartSkill(this, onSkillComplete);
        _lastSkillTime = now;
        _isUsingSkill = false;
    }
    else if (distance < 4f && now >= _lastSweepTime + SweepCooldown)
    {
        _lastSweepTime = now;
        _isUsingSkill = true;
        _sweepSkill.StartSkill(this, onSkillComplete);
        _lastSkillTime = now;
        _isUsingSkill = false;
    }
    else
    {
        onSkillComplete?.Invoke();
    }
}


    public override bool IsPlayerInSkillRange()
    {
        return _player != null && Vector2.Distance(transform.position, _player.position) <= 10f;
    }
}
