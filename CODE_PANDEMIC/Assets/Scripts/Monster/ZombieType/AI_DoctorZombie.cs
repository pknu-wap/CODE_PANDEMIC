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
    public float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    public Transform Player => _player;
    private ISkillBehavior _skill;
    public ISkillBehavior Skill { get; private set; }
    
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