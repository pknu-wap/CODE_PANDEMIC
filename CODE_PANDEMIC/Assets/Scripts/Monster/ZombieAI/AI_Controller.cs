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
    public Animator _animator;
    [SerializeField] private AI_Fov _aiFov;
    [SerializeField] public AIPath _aiPath;
    [SerializeField] public AIDestinationSetter _destinationSetter;

    private AI_IState _currentState;
    public virtual ISkillBehavior Skill { get { return null; } }

    private Coroutine _aiDamageCoroutine;

    private bool _isAttacking;

    private float _radius = 0.41f;
    private float _height = 0.01f;
    private float _pickNextWaypointDist = 1.2f;
    private Vector3 _gravity = new(0, 0, 0);

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
        _player = playerComponent.transform;

        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        ChangeState(new AI_StateIdle(this));
        _state = AI_State.Idle;

        _aiPath = GetComponent<AIPath>();
        _destinationSetter = GetComponent<AIDestinationSetter>();

        ConfigureAllAIPaths();
        AssignDestinations();

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
        if (_currentState is not AI_StateAttack)
        {
            Vector3 scale = transform.localScale;
            if (_player.position.x > transform.position.x)
            {
                scale.x = -Mathf.Abs(scale.x);
            }
            else
            {
                scale.x = Mathf.Abs(scale.x);
            }
            transform.localScale = scale;
        }
    
        _currentState?.OnFixedUpdate();    
    }

    public void UpdateFovDirection()
    {
        if (_aiFov != null)
        {
            float angle = transform.localScale.x < 0 ? 0f : 180f;
            _aiFov.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void ChasePlayer()
    {
        _aiPath.canMove = true;
    }

    public void StopMoving()
    {
        _aiPath.canMove = false;
    }

    public void ChangeState(AI_IState newState)
    {
    if (_currentState != null && _currentState.GetType() == newState.GetType()) //중복 전환 무시
        {
        return;
        }

        _currentState?.OnExit();
        _currentState = newState;
        _currentState?.OnEnter();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (Health <= 0f && _currentState is not AI_StateDie)
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
            return distance <= doctor.SweepRange*0.7f;
        }
        if (this is AI_NurseZombie nurse)
        {
            return distance <= nurse.SyringeRange * 0.8f;
        }
        if (this is AI_PatientZombie)
        {
            return distance <= 7.5f;
        }
        return false;
    }

    public bool IsAttacking()
    {
        return _isAttacking;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        _aiPath.canMove = false;
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && !(_currentState is AI_StateAttack))
        {
            StartAttack();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StopAttack();
    }
    

    private IEnumerator ZombieColliderAttack(PlayerController player)
    {
        WaitForSeconds wait = new WaitForSeconds(AttackDelay);
        while (_isAttacking)
        {
            if (player == null)
                yield break;
            Debug.Log($"[ZombieColliderAttack] {gameObject.name} 공격: {player.name}에게 {Damage} 데미지");
            yield return wait;
        }
        _aiDamageCoroutine = null;
    }

    public void StartAttack()
    {
        if (_isAttacking) return;
        _isAttacking = true;
        ChangeState(new AI_StateAttack(this));
        PlayerController player = _player?.GetComponent<PlayerController>();
        if (player != null && _aiDamageCoroutine == null)
        {
            _aiDamageCoroutine = StartCoroutine(ZombieColliderAttack(player));
        }
    }

    public void StopAttack()
    {
        _isAttacking = false;
        if (_aiDamageCoroutine != null && !IsAttacking())
        {
            StopCoroutine(_aiDamageCoroutine);
            _aiDamageCoroutine = null;
            ChangeState(new AI_StateIdle(this));
        }
    }

        private void ConfigureAllAIPaths()
    {
            _aiPath.radius = _radius;
            _aiPath.height = _height;
            _aiPath.maxSpeed = MoveSpeed;
            _aiPath.pickNextWaypointDist = _pickNextWaypointDist;
            _aiPath.orientation = OrientationMode.YAxisForward; // 2D 모드
            _aiPath.enableRotation = false;
            _aiPath.gravity = _gravity;
    }
    private void AssignDestinations()
    {
            _destinationSetter.target = _player;
        }
    }