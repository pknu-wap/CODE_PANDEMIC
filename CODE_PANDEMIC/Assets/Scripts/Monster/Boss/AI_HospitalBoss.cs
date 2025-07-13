using System.Collections;
using UnityEngine;

public class AI_HospitalBoss : AI_BossController
{
    [SerializeField]
    public LayerMask TargetLayer;
    [SerializeField]
    public GameObject _syringePrefab;
    public ThrowSkillData _throwSkillData;
    public SweepSkillData _sweepSkillData;
    public DashSkillData _dashSkillData;
    private bool _shortAttack;

    public AI_BossThrowVisualizer _syringeVisualizer;
    public override float AiDamage => _monsterData.AttackDamage;
    private AI_BossThrow _throwSkill = new AI_BossThrow();
    private AI_BossSweep _sweepSkill = new AI_BossSweep();
    private AI_BossDash _dashSkill = new AI_BossDash();


    public override ISkillBehavior Skill
    {
        get
        {
            if (_dashSkill != null && _dashSkill.IsReady(this) && IsBerserk == true)
            {
                _shortAttack = false;
                return _dashSkill;
            }
            if (_throwSkill != null && _throwSkill.IsReady(this))
            {
                _shortAttack = false;
                return _throwSkill;
            }
            if (_sweepSkill != null && _sweepSkill.IsReady(this))
            {
                _shortAttack = true;
                return _sweepSkill;
            }
            
            return null;
        }
    }
   
        public override void SetInfo(MonsterData monsterData)
    {
        base.SetInfo(monsterData);
        StartCoroutine(BossStartSequence());
    }
    protected override void Start()
    {
        ChangeState(new AI_StateIdle(this));
        _throwSkill.SetController(this);
        _sweepSkill.SetController(this);
        _dashSkill.SetController(this);
        _syringeVisualizer.SetController(this);
        // if (_monsterData == null)
        // {
        //     _monsterData = new MonsterData();
        //     _monsterData.NameID = "HospitalBoss";
        //     _monsterData.Hp = 1000;
        //     _monsterData.AttackDelay = 5.0f;
        //     _monsterData.DetectionRange = 7.5f;
        //     _monsterData.DetectionAngle = 360;
        //     _monsterData.MoveSpeed = 3.5f;
        //     _monsterData.AttackRange = 2f;
        //     _monsterData.AttackDamage = 20;
        // }
        SettingData();
        base.Start();
        if (!Init())
        {
            enabled = false;
            return;
        }
        _sweepSkill.SetSettings(_sweepSkillData, TargetLayer, this);
        _throwSkill.SetSettings(_throwSkillData, TargetLayer, this);
        _dashSkill.SetSettings(_dashSkillData, TargetLayer, this);
    }

    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();
    }
  
    public override bool IsPlayerInSkillRange()
    {
        float distance = Vector2.Distance(transform.position, _player.position);

        if (_sweepSkill != null && _sweepSkill.IsReady(this) && _shortAttack == true)
            return distance <= _sweepSkillData.Range * 0.7f;
        else
            return distance <= _monsterData.DetectionRange;
    }
    private void SettingData()
    {
        _sweepSkillData = new SweepSkillData
        {
            Cooldown = 15f,
            Range = 4f,
            Angle = 120f,
            Count = 10,
            Interval = 0.5f,
            ChargeDelay = 0.5f
        };
        _throwSkillData = new ThrowSkillData
        {
            Cooldown = 10f,
            Range = 10f,
            ChargeDelay = 0.5f,
            SyringeSpeed = 10f
        };
        _dashSkillData = new DashSkillData
        {
            Cooldown = 20f,
            Width = 5f,
            Speed = 10f,
            ChargeDelay = 0.8f
        };

    }

}
