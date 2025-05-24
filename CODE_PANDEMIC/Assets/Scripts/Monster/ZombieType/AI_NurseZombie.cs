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
    }
    protected override void Start()
    {
        //    if (_monsterData == null)
        // {
        //    _monsterData = new MonsterData();
        //    _monsterData.NameID = "NurseZombie";
        //    _monsterData.Hp = 90;
        //    _monsterData.AttackDelay = 5.0f;
        //    _monsterData.DetectionRange = 7.5f;
        //    _monsterData.DetectionAngle = 180;
        //    _monsterData.MoveSpeed = 3.5f;
        //    _monsterData.AttackRange = 2f;
        //    _monsterData.AttackDamage = 10;
        // }
        _throwSkillData = new ThrowSkillData
        {
            Cooldown = 8f,
            Range = 7f,
            SyringeSpeed = 10f,
            ChargeDelay = 0.5f
        };
        base.Start();
        if (!Init())
        {
            enabled = false;
            return;
        }
        _skill = new AI_ThrowSkill();
        _skill.SetSettings(_throwSkillData, TargetLayer, this);

    }
    public override bool IsPlayerInSkillRange()
    {
        if (_player == null) return false;
        float distance = Vector2.Distance(transform.position, _player.position);
        return distance <= _throwSkillData.Range * 0.8f;
    }
}