using UnityEngine;

public class AI_NurseZombie : AI_Controller
{
    public GameObject _syringePrefab;
    public LayerMask TargetLayer;
    public AI_LineVisualizer _visualizer;
    public ThrowSkillData _throwSkillData;

    public override float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    private ISkillBehavior _skill;
    public override ISkillBehavior Skill { get { return _skill; } }
    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();

        _throwSkillData = new ThrowSkillData
        {
            Cooldown = 8f,
            Range = 7f,
            SyringeSpeed = 10f,
            ChargeDelay = 0.5f
        };

        ThrowSkillSettings throwSkillSettings = new ThrowSkillSettings
        {
            Data = _throwSkillData,
            SyringePrefab = _syringePrefab,
            Visualizer = _visualizer
        };

        _skill = new AI_ThrowSkill();
        _skill.SetSettings(throwSkillSettings, TargetLayer, this);
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
        return Vector2.Distance(transform.position, _detection.Player.position) <= _throwSkillData.Range * 0.8f;
    }
}