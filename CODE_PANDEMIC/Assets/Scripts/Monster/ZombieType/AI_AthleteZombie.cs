using UnityEngine;

public class AI_AthleteZombie : AI_Controller
{
    public LayerMask TargetLayer;
    public float SkillCooldown = 8f;
    public float SkillChargeDelay = 0.5f;
    public float DashSpeed = 5f;
    public float DashWidth = 2f;

    public override float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    public Transform Player => _player.transform;

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
        _monsterData.NameID = "AhtleteZombie";
        _monsterData.Hp = 110;
        _monsterData.AttackDelay = 5.0f;
        _monsterData.DetectionRange = 3f;
        _monsterData.DetectionAngle = 120;
        _monsterData.MoveSpeed = 3.5f;
        _monsterData.AttackRange = 2f;
        _monsterData.AttackDamage = 20;
    }
    base.Start();
    if (!Init()){
        enabled = false;
        return;
    }

    _skill = new AI_DashSkill();
}
    public override bool IsPlayerInSkillRange()
        {
            if (_player == null) return false;

            float distance = Vector2.Distance(transform.position, _player.position);
            return distance <= _monsterData.AttackRange;
        }

}
