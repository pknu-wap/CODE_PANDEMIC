using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class AI_ThunderZombie : AI_Controller
{
    [SerializeField] private GameObject attackColliderPrefab;
    [SerializeField] private Transform hitboxSpawnPoint;
    [SerializeField] private GameObject thunderPrefab;

    public LayerMask TargetLayer;

    public override float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;

    private AI_ThunderSkill _thunderSkill;
    private AI_PatientAttack _patientAttack;

    public static event Action OpenWorksiteDoor;

    public override ISkillBehavior Skill
    {
        get
        {
            if (_patientAttack == null)
            {
                PatientAttackSkillData patientAttackSkillData = new PatientAttackSkillData
                {
                    Cooldown = 2f,
                    Duration = 0.1f
                };
                _patientAttack = new AI_PatientAttack();
                _patientAttack.SetSettings(patientAttackSkillData, TargetLayer, this);
            }
            return _patientAttack;
        }
    }

    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();

        ThunderSkillData thunderSkillData = new ThunderSkillData
        {
            Cooldown = 10f,
            DelayBeforeStrike = 1f,
            IntervalBetweenStrikes = 1.0f,
            StrikeCount = 3,
            StrikeRadius = 0.5f
        };

        _thunderSkill = new AI_ThunderSkill();
        _thunderSkill.SetSettings(thunderSkillData, TargetLayer, this);

        PatientAttackSkillData patientAttackSkillData = new PatientAttackSkillData
        {
            Cooldown = 2f,
            Duration = 0.1f
        };
        _patientAttack = new AI_PatientAttack();
        _patientAttack.SetSettings(patientAttackSkillData, TargetLayer, this);
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

    public override bool IsPlayerInSkillRange()
    {
        if (_detection.Player == null)
            return false;
        if (_thunderSkill != null && _thunderSkill.IsReady(this))
            return Vector2.Distance(transform.position, _detection.Player.position) <= AttackRange;
        else
            return Vector2.Distance(transform.position, _detection.Player.position) <= 0.6f;
    }

    public void AttackHit()
    {
        _patientAttack?.StartSkill(this, null);
    }
    protected override void Dielogic()
    {
        base.Dielogic();
        OpenWorksiteDoor?.Invoke();
        Debug.Log($"{AIName} has died.");
    }
}
