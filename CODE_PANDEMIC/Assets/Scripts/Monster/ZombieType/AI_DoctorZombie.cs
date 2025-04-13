using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class AI_DoctorZombie : AI_Controller
{
    public float SweepRange = 2f;
    public float SweepAngle = 90f;
    public int SweepCount = 5;
    public float SweepInterval = 0.5f;
    public float SweepCooldown = 5f;
    public LayerMask TargetLayer;
    public float AiDamage => _aiDamage;
    public string AIName => _aiName;
    public Transform Player => _player;
    private ISkillBehavior _skill;
    public ISkillBehavior Skill { get; private set; }
    protected override void Awake()
    {
        _aiName = "DoctorZombie";
        _aiHealth = 100f;
        _aiDamage = 10f;
        _aiMoveSpeed = 100f;
        _aiDetectionRange = 7.5f;
        _aiDetectionAngle = 120f;
        _aiAttackRange = 2f;
        _aiDamageDelay = 5f;
        _aiDetectionAngle = 120f;
        _aiDetectionRange = 7.5f;
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();
    }
    protected override void Start()
    {
        // 기본 Init()은 부모에서 실행되도록 두고, 이후 스킬 할당
        if (!Init())
        {
            enabled = false;
            return;
        }
        // DoctorZombie 전용 스킬 할당
        Skill = new AI_SweepSkill(); // Assigning the skill
    }
}