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
    public AI_SweepVisualizer SweepVisualizer;
    public AI_SweepSkill SweepSkill;

    public float AiDamage => _aiDamage;
    public string AIName => _aiName;
    public Transform Player => _player;
    private ISkillBehavior _skill;
    public override ISkillBehavior Skill { get { return _skill; } }
    protected override void Awake()
    {
        _aiName = "DoctorZombie";
        _aiHealth = 100f;
        _aiDamage = 10f;
        _aiMoveSpeed = 100f;
        _aiDetectionRange = 7.5f;
        _aiDetectionAngle = 120f;
        _aiAttackRange = 2f;
        _aiDamageDelay = 5f;
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();
    }
    protected override void Start()
    {
        if (!Init())
        {
            enabled = false;
            return;
        }
        if (SweepVisualizer != null)
        {
            SweepVisualizer.Hide();
            SweepVisualizer._doctor = this;
        }
        if (SweepVisualizer != null && Player != null)
    {
        SweepVisualizer.playerTransform = Player;
    }
        
        _skill = new AI_SweepSkill();
    }
}