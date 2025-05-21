using UnityEngine;

public class AI_DoctorZombie : AI_Controller
{
    public LayerMask TargetLayer;
    public override float AiDamage => _monsterData.AttackDamage;
    public AI_SweepVisualizer _sweepVisualizer;
    public SweepSkillData _sweepSkillData;

    private ISkillBehavior _skill;
    public override ISkillBehavior Skill => _skill;

    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();
    }

    protected override void Start()
    {
        // if (_monsterData == null)
        // {
        //     _monsterData = new MonsterData
        //     {
        //         NameID = "DoctorZombie",
        //         Hp = 110,
        //         AttackDelay = 5.0f,
        //         DetectionRange = 7.5f,
        //         DetectionAngle = 180,
        //         MoveSpeed = 3.5f,
        //         AttackRange = 2f,
        //         AttackDamage = 10
        //     };
        // }

        _sweepSkillData = new SweepSkillData
        {
            Cooldown = 15f,
            Range = 2f,
            Angle = 90f,
            Count = 5,
            Interval = 0.5f,
            ChargeDelay = 2f
        };

        base.Start();
        if (!Init())
        {
            enabled = false;
            return;
        }

        if (_sweepVisualizer != null)
        {
            _sweepVisualizer.Hide();
        }

        _skill = new AI_SweepSkill();
        _skill.SetSettings(_sweepSkillData, TargetLayer, this);
    }

    public override bool IsPlayerInSkillRange()
    {
        if (_player == null) return false;
        float distance = Vector2.Distance(transform.position, _player.position);
        return distance <= _sweepSkillData.Range * 0.7f;
    }
}
