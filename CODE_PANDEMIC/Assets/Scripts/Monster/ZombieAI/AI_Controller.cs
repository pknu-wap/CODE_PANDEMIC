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
    public AIPath _aiPath;
    public AIDestinationSetter _destinationSetter;

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
        TryDetectPlayer();
        UpdateFovDirection();
        _currentState?.OnUpdate();

        if (_player == null || Skill == null) return;

        float distance = Vector2.Distance(transform.position, _player.position);
        bool inRange = IsPlayerInSkillRange();
        bool skillReady = Skill.IsReady(this);

        if (!_isAttacking && inRange)
        {
            StartAttack();
        }
        else if (_isAttacking && (!inRange || !skillReady))
        {
            StopAttack();
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
    private void TryDetectPlayer()
    {
    foreach (var obj in _aiFov.GetDetectedObjects())
    {
        if (obj.TryGetComponent<PlayerStatus>(out _))
        {
            _player = obj.transform;
            _destinationSetter.target = _player;
            break;
        }
    }
    if (_player == null) _destinationSetter.target = null;
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
        _animator.SetTrigger("Walk");
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

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
    //    _aiPath.canMove = false;
    //    PlayerStatus player = other.GetComponent<PlayerStatus>();
    //    _player = player.transform;
    //    if (player != null && !(_currentState is AI_StateAttack))
    //    {
    //        StartAttack();
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    StopAttack();
    //}


    private IEnumerator ZombieAttackCoroutine(PlayerStatus player)
    {
        WaitForSeconds wait = new(AttackDelay);
        while (_isAttacking)
        {
            if (player == null) break;
            PerformAttack();
            yield return wait;
        }

        _aiDamageCoroutine = null;
    }
    public void StartAttack()
    {
        if (_isAttacking || _player == null) return;

        _isAttacking = true;
        StopMoving();
        ChangeState(new AI_StateAttack(this));

        if (_aiDamageCoroutine == null && _player.TryGetComponent(out PlayerStatus player))
        {
            _aiDamageCoroutine = StartCoroutine(ZombieAttackCoroutine(player));
        }
    }

    public void StopAttack()
    {
        if (!_isAttacking) return;

        _isAttacking = false;
        _aiPath.canMove = true;
        ChangeState(new AI_StateIdle(this));

        if (_aiDamageCoroutine != null)
        {
            StopCoroutine(_aiDamageCoroutine);
            _aiDamageCoroutine = null;
        }
    }
    public void PerformAttack()
    {
        Vector2 attackCenter = (Vector2)transform.position + new Vector2(transform.localScale.x * 0.5f, 0f);
        float attackRadius = 0.7f;
        LayerMask playerMask = LayerMask.GetMask("Player");

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCenter, attackRadius, playerMask);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<PlayerStatus>(out var player))
            {
                player.OnDamaged(gameObject, Damage);
            }
        }
    }

    private void ConfigureAllAIPaths()
    {
            _aiPath.radius = _radius;
            _aiPath.height = _height;
            // _aiPath.maxSpeed = MoveSpeed;
            _aiPath.pickNextWaypointDist = _pickNextWaypointDist;
            _aiPath.orientation = OrientationMode.YAxisForward; // 2D 모드
            _aiPath.enableRotation = false;
            _aiPath.gravity = _gravity;
    }
    private void AssignDestinations()
    {
        _destinationSetter.target = null;
    }
    }