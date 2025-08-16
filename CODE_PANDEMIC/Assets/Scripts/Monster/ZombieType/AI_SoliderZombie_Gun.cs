using System;
using UnityEngine;

public class AI_SoliderZombie_Gun : AI_Controller
{
    public LayerMask _targetLayer;
    public GameObject _bulletPrefab;
    public override float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    private GunSkillData _gunSkillData;
    private ISkillBehavior _skill;
    public override ISkillBehavior Skill { get { return _skill; } }
    protected override void Awake()
    {
        _targetLayer = LayerMask.GetMask("Player");
        base.Awake();
        _gunSkillData = new GunSkillData
        {
            Cooldown = 10f,
            FireRate = 0.25f,
            StrikeCount = 20,
            Range = 10f,
            Damage = 15f,
            BulletSpeed = 12f,
            BulletPrefab = _bulletPrefab
        };
        _skill = new AI_Gunskill();
        _skill.SetSettings(_gunSkillData, _targetLayer, this);
    }
    protected override void Start()
    {
    {
        _monsterData = new MonsterData();   
        _monsterData.NameID = "SoliderZombie_g";
        _monsterData.Hp = 100;
        _monsterData.AttackDelay = 5.0f;
        _monsterData.DetectionRange = 7.5f;
        _monsterData.DetectionAngle = 180;
        _monsterData.MoveSpeed = 3.5f;
        _monsterData.AttackRange = 2f;
        _monsterData.AttackDamage = 10;
    }    
    // 테스트용 값, 실제 값은 추후 정할 예정
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
        
        return Vector2.Distance(transform.position, _detection.Player.position) <= _gunSkillData.Range * 0.8f;
    }
}