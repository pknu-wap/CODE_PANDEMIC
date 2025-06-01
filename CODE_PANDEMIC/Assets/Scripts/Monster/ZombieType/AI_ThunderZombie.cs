using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class AI_ThunderZombie : AI_Controller
{
    public float _thunderCoolDown = 10f;
    public float _thunderDelay = 1f;
    public float _attackCooldown = 2f;
    public float _attackDuration = 0.1f;

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
            if (_thunderSkill != null && _thunderSkill.IsReady(this))
                return _thunderSkill;

            if (_patientAttack == null)
            {
                _patientAttack = new AI_PatientAttack(attackColliderPrefab, hitboxSpawnPoint, _attackCooldown, _attackDuration);
            }

            return _patientAttack;
        }
    }

    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();
    }

    protected override void Start()
    {
        // if (_monsterData == null)
        // {
        //     _monsterData = new MonsterData();
        //     _monsterData.NameID = "ThunderZombie";
        //     _monsterData.Hp = 500;
        //     _monsterData.AttackDelay = 2f;
        //     _monsterData.DetectionRange = 10f;
        //     _monsterData.DetectionAngle = 180;
        //     _monsterData.MoveSpeed = 4f;
        //     _monsterData.AttackRange = 10f;
        //     _monsterData.AttackDamage = 30;
        // }
        base.Start();

        if (!Init())
        {
            enabled = false;
            return;
        }

        _thunderSkill = new AI_ThunderSkill(thunderPrefab, TargetLayer);
        _patientAttack = new AI_PatientAttack(attackColliderPrefab, hitboxSpawnPoint, _attackCooldown, _attackDuration);

    }

    public override bool IsPlayerInSkillRange()
    {
        if (_player == null)
            return false;

        float distance = Vector2.Distance(transform.position, _player.position);

        if (_thunderSkill != null && _thunderSkill.IsReady(this))
            return IsPlayerDetected();
        else
            return distance <= 0.6f;
    }

    public void AttackHit()
    {
        _patientAttack?.StartSkill(this, null);
    }
    public override void Die() // 여기에다가 번개좀비 작업하면 됩니다.
    {
        base.Die();
        Skill?.StopSkill();
        StopMoving();
        ChangeState(new AI_StateDie(this));
        _animator.SetTrigger("Die");

        OpenWorksiteDoor?.Invoke();

        Debug.Log($"{AIName} has died.");
    }
}
