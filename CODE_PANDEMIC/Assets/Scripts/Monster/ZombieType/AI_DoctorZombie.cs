using UnityEngine;

public class AI_DoctorZombie : AI_Controller
{
    public LayerMask TargetLayer;
    public override float AiDamage => _monsterData.AttackDamage;
    public SweepSkillData _sweepSkillData;

    private ISkillBehavior _skill;
    public override ISkillBehavior Skill => _skill;

    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();

        _sweepSkillData = new SweepSkillData
        {
            Cooldown = 15f,
            Range = 2f,
            Angle = 90f,
            Count = 5,
            Interval = 0.5f,
            ChargeDelay = 2f
        };

        _skill = new AI_SweepSkill();
        _skill.SetSettings(_sweepSkillData, TargetLayer, this);
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

    public override bool IsPlayerInSkillRange()
    {
        if (_detection.Player == null)
            return false;
        return Vector2.Distance(transform.position, _detection.Player.position) <= _sweepSkillData.Range * 0.9f;
    }
}
