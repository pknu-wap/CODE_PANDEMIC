using UnityEngine;

public class AI_SoliderZombie_Grenade : AI_Controller
{
    public LayerMask _targetLayer;
    public GameObject _grenadePrefab;
    public GameObject _explosionEffect;

    public override float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;

    private GrenadeSkillData _grenadeSkillData;
    private ISkillBehavior _skill;

    public override ISkillBehavior Skill => _skill;

    protected override void Awake()
    {
        _targetLayer = LayerMask.GetMask("Player");
        base.Awake();

        _grenadeSkillData = new GrenadeSkillData
        {
            Cooldown = 10f,
            ThrowDelay = 0.5f,
            Damage = 20f,
            ExplosionRadius = 3f,
            ExplosionDelay = 1.5f,
            GrenadePrefab = _grenadePrefab,
            ExplosionEffect = _explosionEffect
        };

        _skill = new AI_GrenadeSkill();
        _skill.SetSettings(_grenadeSkillData, _targetLayer, this);
    }

    protected override void Start()
    {
        {
            _monsterData = new MonsterData();
            _monsterData.NameID = "SoliderZombie_t";
            _monsterData.Hp = 100;
            _monsterData.AttackDelay = 5.0f;
            _monsterData.DetectionRange = 7.5f;
            _monsterData.DetectionAngle = 180;
            _monsterData.MoveSpeed = 3.5f;
            _monsterData.AttackRange = 2f;
            _monsterData.AttackDamage = 10;
        }
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

        return Vector2.Distance(transform.position, _detection.Player.position) 
            <= _monsterData.DetectionRange * 0.8f;
    }
}
