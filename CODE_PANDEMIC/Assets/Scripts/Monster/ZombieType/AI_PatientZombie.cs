 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AI_PatientZombie : AI_Controller
{
    public LayerMask TargetLayer;
    public override float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    [SerializeField] private GameObject attackColliderPrefab;
    [SerializeField] private Transform hitboxSpawnPoint;

    private ISkillBehavior _skill;    
    public override ISkillBehavior Skill { get { return _skill; } }
    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();

        PatientAttackSkillData patientAttackSkillData = new PatientAttackSkillData
        {
            Cooldown = 2f,
            Duration = 0.5f,
            HitboxPrefab = attackColliderPrefab,
            SpawnPoint = hitboxSpawnPoint
        };
        _skill = new AI_PatientAttack();
        _skill.SetSettings(patientAttackSkillData, TargetLayer, this);
    }
    protected override void Start()
    {
        base.Start();
        if (!Init())
        {
            enabled = false;
            return;
        }
    }
    public void AttackHit()
    {
        _skill.StartSkill(this, null);
    }
    public override bool IsPlayerInSkillRange()
    {
        if (_detection.Player == null)
            return false;
        return Vector2.Distance(transform.position, _detection.Player.position) <= AttackRange * 0.3f;
    }
}
