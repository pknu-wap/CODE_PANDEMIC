using System;
using System.Collections.Generic;
using UnityEngine;

public enum AI_State
{
    Idle,
    Walk,
    Attack,
    Dead
}

[RequireComponent(typeof(AI_Movement), typeof(AI_Detection), typeof(AI_Combat))]
public class AI_Controller : AI_Base
{
    public AI_Movement _movement;
    public AI_Detection _detection;
    public AI_Combat _combat;

    protected Rigidbody2D _rb;
    public Animator _animator;
    private EnemyDamageEffect _damageEffect;

    protected AI_IState _currentState;
    private Dictionary<Type, AI_IState> _states;

    public bool _isDead = false;

    public virtual ISkillBehavior Skill => null;
    public virtual float AiDamage => 0f;

    protected override void Awake()
    {
        base.Awake();
        _movement = GetComponent<AI_Movement>();
        _detection = GetComponent<AI_Detection>();
        _combat = GetComponent<AI_Combat>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _damageEffect = Utils.GetOrAddComponent<EnemyDamageEffect>(gameObject);

        _states = new Dictionary<Type, AI_IState>
        {
            { typeof(AI_StateIdle), new AI_StateIdle() },
            { typeof(AI_StateWalk), new AI_StateWalk() },
            { typeof(AI_StateAttack), new AI_StateAttack() },
            { typeof(AI_StateDie), new AI_StateDie() }
        };
    }

    protected virtual void Start()
    {
        if (!Init())
        {
            enabled = false;
            return;
        }

        _movement.Configure(MoveSpeed);
        if (Skill != null)
        {
            _combat.Skill = Skill;
        }
        ChangeState<AI_StateIdle>();
    }

    public override bool Init()
    {
        if (_rb != null) _rb.freezeRotation = true;
        return true;
    }

    private void Update()
    {
        if (_isDead) return;
        _detection.DetectPlayer(OnPlayerDetected);
        _currentState?.OnUpdate();
        bool inRange = IsPlayerInSkillRange();
        bool skillReady = Skill != null && Skill.IsReady(this);
        if (!_movement._isUsingSkill && inRange && skillReady && _detection.IsPlayerDetected)
        {
            _combat.StartAttack(OnAttackStarted);
        }
    }

    private void FixedUpdate()
    {
        if (_isDead) return;
        _movement.UpdateDirection(_detection.Player);
        _movement.UpdateFovDirection();
        _currentState?.OnFixedUpdate();
    }

    private void OnPlayerDetected(Transform player)
    {
        _movement.SetTarget(player);
        if (player == null)
        {
            _movement.StopMoving();
        }
        else
        {
            if (_currentState is AI_StateIdle)
            {
                ChangeState<AI_StateWalk>();
            }
        }
    }

    public void ChangeState<T>() where T : AI_IState
    {
        if (!_states.TryGetValue(typeof(T), out var newState)) return;
        if (_currentState?.GetType() == typeof(T) || _currentState is AI_StateDie) return;
        
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter(this);
    }

    public override void TakeDamage(int amount)
    {
        if (_isDead) return;
        base.TakeDamage(amount);
        if (Health > 0)
            _damageEffect.CallDamageFlash();

        if (_currentState is not AI_StateDie)
        {
            var player = FindObjectOfType<PlayerStatus>().transform;
            ForceDetectTarget(player);
        }
    }

    public override void Die()
    {
        Dielogic();
    }

    protected virtual void Dielogic()
    {
        if (_isDead) return;
        _isDead = true;
        Managers.Game.AddZombieKillCount();
        _combat.StopSkill();
        _rb.velocity = Vector2.zero;
        _movement.StopMoving();
        ChangeState<AI_StateDie>();
    }

    public void ForceDetectTarget(Transform player)
    {
        _detection.ForceDetectTarget(player);
        _movement.SetTarget(player);
        ChangeState<AI_StateWalk>();
    }

    private void OnAttackStarted()
    {
        _movement.StopMoving();
        ChangeState<AI_StateAttack>();
    }

    public void OnAttackStopped()
    {
        _movement.ChasePlayer();
        ChangeState<AI_StateIdle>();
    }

    public void ChasePlayer() => _movement.ChasePlayer();
    public void StopMoving() => _movement.StopMoving();
    public bool IsAttacking() => _combat.IsAttacking;

    public virtual bool IsPlayerInSkillRange()
    {
        if(_detection.Player == null) return false;
        return Vector2.Distance(transform.position, _detection.Player.position) <= 1f;
    }
}

