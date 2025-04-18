using UnityEngine;

public class AI_NurseZombie : AI_Controller
{
    public GameObject _syringePrefab;
    public float SkillCooldown = 8f;
    public float SkillChargeDelay = 1f;    
    public float SyringeSpeed = 10f;  
    public float SyringeRange = 10f;

    public LayerMask TargetLayer;
    public AI_ThrowVisualizer ThrowVisualizer;
    public AI_ThrowSkill ThrowSkill;

    public float AiDamage => _aiDamage;
    public string AIName => _aiName;
    public Transform Player => _player;
    private ISkillBehavior _skill;
    public override ISkillBehavior Skill { get { return _skill; } }
    protected override void Awake()
    {
        _aiName = "NurseZombie";
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
        _skill = new AI_ThrowSkill();
    }
}