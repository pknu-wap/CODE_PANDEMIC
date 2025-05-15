 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AI_PatientZombie : AI_Controller
{
    public LayerMask TargetLayer;
    public GameObject patientAttack;
    public override float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    public Transform Player => _player.transform;
    [SerializeField] private GameObject attackColliderPrefab;
    [SerializeField] private Transform hitboxSpawnPoint;
    private float _skillCooldown = 5f;
    private float _duration = 0.2f;

    private ISkillBehavior _skill;    
    public override ISkillBehavior Skill { get { return _skill; } }
    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();
    }
    protected override void Start()
    {
    //     if (_monsterData == null)
    // {
    //     _monsterData = new MonsterData();
    //     _monsterData.NameID = "PatientZombie";
    //     _monsterData.Hp = 100;
    //     _monsterData.AttackDelay = 5.0f;
    //     _monsterData.DetectionRange = 7.5f;
    //     _monsterData.DetectionAngle = 180;
    //     _monsterData.MoveSpeed = 3.5f;
    //     _monsterData.AttackRange = 2f;
    //     _monsterData.AttackDamage = 10;
    // }        
        base.Start();
        if (!Init())
        {
            enabled = false;
            return;
        }
        _skill = new AI_PatientAttack(attackColliderPrefab, hitboxSpawnPoint, _skillCooldown, _duration);
    }
    public void AttackHit()
    {
        _skill.StartSkill(this, null);
    }
    public override bool IsPlayerInSkillRange()
    {
        if (_player == null) return false;
        float distance = Vector2.Distance(transform.position, _player.position);
        return distance <= _monsterData.AttackRange * 0.7f;
    }
}
