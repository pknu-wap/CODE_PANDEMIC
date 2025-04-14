using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum AI_State
{
    Idle,
    Walk,
    Attack,
    Dead
}

public class AI_Controller : AI_Base
{
    public Transform _player;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    private AI_Fov _aiFov;
    public AIPath _aiPath;

    private AI_IState _currentState;
    public virtual ISkillBehavior Skill { get { return null; } }

    private Coroutine _aiDamageCoroutine;

    private bool _isAttacking;

    protected virtual void Awake(){}

    protected virtual void Start()
    {
        if (!Init())
        {
            enabled = false;
            return;
        }
    }

    public override bool Init()
    {
        PlayerController playerComponent = FindObjectOfType<PlayerController>();
        if (playerComponent == null)
        {
            Debug.LogError("Player 없음");
            return false;
        }
        _player = playerComponent.transform;

        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody 없음");
            return false;
        }
        _rb.freezeRotation = true;

        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
        {
            Debug.LogError("SpriteRenderer 없음");
            return false;
        }

        _aiFov = GetComponentInChildren<AI_Fov>();
        if (_aiFov == null)
        {
            Debug.LogError("AI_Fov 없음");
            return false;
        }

        _aiPath = GetComponent<AIPath>();
        if (_aiPath == null)
        {
            Debug.LogError("AIPath 없음");
            return false;
        }
 

        ChangeState(new AI_StateIdle(this));
        _state = AI_State.Idle;
        return true;
    }

    private void Update()
    {
        if (_player == null)
            return;

        UpdateFovDirection();
        _currentState?.OnUpdate();
        if (_currentState is AI_StateWalk && IsPlayerInSkillRange() && Skill != null && Skill.IsReady(this))
        {
            ChangeState(new AI_StateAttack(this));
        }
    }

    private void FixedUpdate()
    {
        if (_player == null)
            return;

        _renderer.flipX = _player.position.x < transform.position.x;
        _currentState?.OnFixedUpdate();
    }

    public void UpdateFovDirection()
    {
        if (_aiFov != null)
        {
            float angle = _renderer.flipX ? 180f : 0f;
            _aiFov.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void ChasePlayer()
    {
        if (!_isAttacking)
        {
            _aiPath.canMove = true;
        }
    }

    public void StopMoving()
    {
        _aiPath.canMove = false;
    }

    public void ChangeState(AI_IState newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter();
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        if (_aiHealth <= 0f && _currentState is not AI_StateDie)
        {
            ChangeState(new AI_StateDie(this));
        }
    }

    public bool IsPlayerDetected()
    {
        return _aiFov != null && _aiFov.GetDetectedObjects().Contains(_player.gameObject);
    }

    public bool IsPlayerInSkillRange()
    {
        if (_player == null) return false;
        float distance = Vector2.Distance(transform.position, _player.position);
        if (this is AI_DoctorZombie doctor)
        {
            return distance <= doctor.SweepRange * 0.5f;
        }
        return false;
    }

    public bool IsAttacking()
    {
        return _isAttacking;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            StartAttack();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_aiDamageCoroutine != null)
        {
            StopAttack();
        }
    }
    

    private IEnumerator ZombieColliderAttack(PlayerController player)
    {
        WaitForSeconds wait = new WaitForSeconds(_aiDamageDelay);
        while (_isAttacking)
        {
            if (player == null)
                yield break;

            Debug.Log($"{_aiName}이 {_aiDamage} 데미지 주는 공격 실행 중");

            yield return wait;
        }
        _aiDamageCoroutine = null;
    }

    public void StartAttack()
    {
        if (_isAttacking) return;
        _isAttacking = true;
        PlayerController player = _player?.GetComponent<PlayerController>();
        if (player != null && _aiDamageCoroutine == null)
        {
            _aiDamageCoroutine = StartCoroutine(ZombieColliderAttack(player));
        }
    }

    public void StopAttack()
    {
        _isAttacking = false;
        if (_aiDamageCoroutine != null)
        {
            StopCoroutine(_aiDamageCoroutine);
            _aiDamageCoroutine = null;
        }
    }
}
