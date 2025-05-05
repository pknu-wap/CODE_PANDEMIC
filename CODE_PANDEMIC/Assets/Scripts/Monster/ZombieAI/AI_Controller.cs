using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

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
    public bool _isUsingSkill; 

    private float _radius = 0.41f;
    private float _height = 0.01f;
    private float _pickNextWaypointDist = 1.2f;
    private Vector3 _gravity = new(0, 0, 0);

    private float _lostPlayerTimer = 0f;
    private float _playerLostDelay = 1f;
    protected virtual void Awake() { }

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

        if (!_isAttacking && inRange && skillReady && !_isUsingSkill)
        {
            StartAttack();
        }
        else if (_isAttacking && (!inRange || !skillReady) && !_isUsingSkill)
        {
            StopAttack();
        }
    }

    private void FixedUpdate()
    {
        if (_player == null) return;
        if (_currentState is not AI_StateAttack && !_isUsingSkill)
        {
            Vector3 scale = transform.localScale;
            if (_player.position.x > transform.position.x)
                scale.x = -Mathf.Abs(scale.x);
            else
                scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        _currentState?.OnFixedUpdate();
    }

    private void TryDetectPlayer()
    {
        bool found = false;

        foreach (var obj in _aiFov.GetDetectedObjects())
        {
            if (obj.TryGetComponent<PlayerStatus>(out _))
            {
                _player = obj.transform;
                _destinationSetter.target = _player;
                _lostPlayerTimer = _playerLostDelay;
                found = true;
                break;
            }
        }

        if (!found)
{
    if (_lostPlayerTimer > 0f)
    {
        _lostPlayerTimer -= Time.deltaTime;
    }
    else
    {
        _player = null;
        _destinationSetter.target = null;
        StopMoving();
    }
}
    }

    public void UpdateFovDirection()
    {
        if (_player == null) return;
        Vector2 direction = _player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _aiFov.transform.rotation = Quaternion.Euler(0, 0, angle);
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
        if (_currentState != null && _currentState.GetType() == newState.GetType())
            return;

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
        return _aiFov != null && _aiFov.GetDetectedObjects().Contains(_player?.gameObject);
    }

    public bool IsPlayerInSkillRange()
    {
        if (_player == null) return false;
        float distance = Vector2.Distance(transform.position, _player.position);
        if (this is AI_DoctorZombie doctor)
            return distance <= doctor.SweepRange * 0.7f;
        if (this is AI_NurseZombie nurse)
            return distance <= nurse.SyringeRange * 0.8f;
        if (this is AI_PatientZombie)
            return distance <= 7.5f;
        return false;
    }

    public bool IsAttacking()
    {
        return _isAttacking;
    }

    public void StartAttack()
    {
        if (_isAttacking || _player == null) return;

        _isAttacking = true;
        StopMoving();
        ChangeState(new AI_StateAttack(this));
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

    public void DoSkill()
{
    if (Skill != null && Skill.IsReady(this))
    {
        _isUsingSkill = true;
        _aiPath.canMove = false;

        Skill.StartSkill(this, () => {
            _isUsingSkill = false;
            _aiPath.canMove = true;
        });
    }
}

    private void ConfigureAllAIPaths()
    {
        _aiPath.radius = _radius;
        _aiPath.height = _height;
        _aiPath.maxSpeed = MoveSpeed;
        _aiPath.pickNextWaypointDist = _pickNextWaypointDist;
        _aiPath.orientation = OrientationMode.YAxisForward;
        _aiPath.enableRotation = false;
        _aiPath.gravity = _gravity;
    }

    private void AssignDestinations()
    {
        _destinationSetter.target = null;
    }
}
