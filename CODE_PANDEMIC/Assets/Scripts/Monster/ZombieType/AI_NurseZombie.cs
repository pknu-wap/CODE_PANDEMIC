using UnityEngine;

public class AI_NurseZombie : AI_Controller
{
    public GameObject _syringePrefab;
    public float SkillCooldown = 8f;
    public float SkillChargeDelay = 0.5f;    
    public float SyringeSpeed = 10f;  
    public float SyringeRange = 7f;

    public LayerMask TargetLayer;

    public override float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    public Transform Player => _player.transform;
    private ISkillBehavior _skill;
    public AI_ThrowVisualizer _throwVisualizer;
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
        base.Start();
        if (!Init())
        {
            enabled = false;
            return;
        }
        _skill = new AI_ThrowSkill();
    }
    public override bool IsPlayerInSkillRange()
    {
        if (_player == null) return false;
        float distance = Vector2.Distance(transform.position, _player.position);
        return distance <= SyringeRange * 0.8f;
    }
}