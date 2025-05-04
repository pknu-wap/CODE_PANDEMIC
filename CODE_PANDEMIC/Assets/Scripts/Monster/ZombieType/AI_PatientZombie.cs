 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AI_PatientZombie : AI_Controller
{
    public LayerMask TargetLayer;
    public GameObject patientAttack;
    public float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    public Transform Player => _player.transform;
    [SerializeField] private GameObject attackColliderPrefab;
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
        _skill = new AI_PatientAttack(attackColliderPrefab);
        if (!Init())
        {
            enabled = false;
            return;
        }
    }
    public void AttackHit()
    {
        Skill?.StartSkill(this, null);
    }
}
