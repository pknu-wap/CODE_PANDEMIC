using UnityEngine;

public class AI_DoctorZombie : AI_Controller
{
    public float SweepRange = 2f;
    public float SweepAngle = 90f;
    public int SweepCount = 5;
    public float SweepInterval = 0.5f;
    public float SkillCooldown = 15f;
    public float SkillChargeDelay = 2f;
    public LayerMask TargetLayer;

    public override float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    public Transform Player => _player.transform;

    [SerializeField] public AI_SweepVisualizer _sweepVisualizer;

    private ISkillBehavior _skill;
    public override ISkillBehavior Skill => _skill;

    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();
    }
    protected override void Start()
{

    if (_monsterData == null)
    {
        _monsterData = new MonsterData();
        _monsterData.NameID = "DoctorZombie";
        _monsterData.Hp = 110;
        _monsterData.AttackDelay = 5.0f;
        _monsterData.DetectionRange = 7.5f;
        _monsterData.DetectionAngle = 180;
        _monsterData.MoveSpeed = 3.5f;
        _monsterData.AttackRange = 2f;
        _monsterData.AttackDamage = 10;
    }
    base.Start();
    if (!Init()){
        enabled = false;
        return;
    }

    if (_sweepVisualizer != null)
    {
        _sweepVisualizer.Hide();
    }

    _skill = new AI_SweepSkill();
}
    public override bool IsPlayerInSkillRange()
        {
            if (_player == null) return false;
            float distance = Vector2.Distance(transform.position, _player.position);
            return distance <= SweepRange * 0.7f;
        }

}
