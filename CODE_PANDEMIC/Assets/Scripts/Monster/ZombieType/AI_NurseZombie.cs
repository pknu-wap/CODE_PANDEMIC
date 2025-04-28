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

    public float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    public Transform Player => _player.transform;
    private ISkillBehavior _skill;
    public override ISkillBehavior Skill { get { return _skill; } }
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
        _monsterData.NameID = "NurseZombie";
        _monsterData.Hp = 90;
        _monsterData.AttackDelay = 5.0f;
        _monsterData.DetectionRange = 7.5f;
        _monsterData.DetectionAngle = 180;
        _monsterData.MoveSpeed = 100.0f;
        _monsterData.AttackRange = 2f;
        _monsterData.AttackDamage = 10;
    }
        base.Start();
        if (!Init())
        {
            enabled = false;
            return;
        }
        _skill = new AI_ThrowSkill();
    }
}