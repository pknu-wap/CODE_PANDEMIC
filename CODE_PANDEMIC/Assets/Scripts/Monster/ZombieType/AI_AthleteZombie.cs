using UnityEngine;

public class AI_AthleteZombie : AI_Controller
{
    public LayerMask TargetLayer;

    public override float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    public Transform Player => _player.transform;

    private ISkillBehavior _skill;
    public override ISkillBehavior Skill => _skill;
    public AI_LineVisualizer _visualizer;
    public DashSkillData _dashSkillData;

    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();
    }
    protected override void Start()
    {
        // if (_monsterData == null)
        // {
        //     _monsterData = new MonsterData();
        //     _monsterData.NameID = "AhtleteZombie";
        //     _monsterData.Hp = 110;
        //     _monsterData.AttackDelay = 5.0f;
        //     _monsterData.DetectionRange = 3f;
        //     _monsterData.DetectionAngle = 120;
        //     _monsterData.MoveSpeed = 3.5f;
        //     _monsterData.AttackRange = 3.5f;
        //     _monsterData.AttackDamage = 20;
        // }
        _dashSkillData = new DashSkillData
        {
            Cooldown = 8f,
            ChargeDelay = 0.5f,
            Speed = 5f,
            Width = 2f,

        };
        base.Start();
        if (!Init())
        {
            enabled = false;
            return;
        }

    _skill = new AI_DashSkill();
    _skill.SetSettings(_dashSkillData, TargetLayer, this);
}
    public override bool IsPlayerInSkillRange()
        {
            if (_player == null) return false;

            float distance = Vector2.Distance(transform.position, _player.position);
            return distance <= _monsterData.AttackRange * 2f;
        }

}
