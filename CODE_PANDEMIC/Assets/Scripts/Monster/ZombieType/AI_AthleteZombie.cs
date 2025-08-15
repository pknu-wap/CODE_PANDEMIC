using UnityEngine;

public class AI_AthleteZombie : AI_Controller
{
    public LayerMask TargetLayer;

    public override float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    public Transform Player => _detection.Player;

    private ISkillBehavior _skill;
    public override ISkillBehavior Skill => _skill;
    public AI_LineVisualizer _visualizer;

    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();

        DashSkillData dashSkillData = new DashSkillData
        {
            Cooldown = 8f,
            ChargeDelay = 0.5f,
            Speed = 5f,
            Width = 2f,
        };

        _skill = new AI_DashSkill();
        _skill.SetSettings(dashSkillData, TargetLayer, this);
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
        return Vector2.Distance(transform.position, _detection.Player.position) <= _monsterData.AttackRange * 2f;;
    }
}
