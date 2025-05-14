using UnityEngine;
using Pathfinding;
using System.Linq;

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
    protected Rigidbody2D _rb;
    protected SpriteRenderer _renderer;
    public Animator _animator;
    [SerializeField] protected AI_Fov _aiFov;
    public AIPath _aiPath;
    protected AIDestinationSetter _destinationSetter;
    EnemyDamageEffect _damageEffect;

    protected AI_IState _currentState;
    protected bool _isAttacking;
    public bool _isUsingSkill;
    public bool _foundPlayer;

    private const float SkillRange = 7.5f;
    public virtual ISkillBehavior Skill => null;
    public virtual float AiDamage => 0f;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _aiPath = GetComponent<AIPath>();
        _destinationSetter = GetComponent<AIDestinationSetter>();
        _damageEffect = Utils.GetOrAddComponent<EnemyDamageEffect>(gameObject);
    }

    protected virtual void Start()
    {
        if (!Init())
        {
            enabled = false;
            return;
        }

        ConfigurePathfinding();
        ChangeState(new AI_StateIdle(this));
        _state = AI_State.Idle;

        _destinationSetter.target = null;
    }

    public override bool Init()
    {
        if (_rb != null) _rb.freezeRotation = true;
        return true;
    }

    private void Update()
    {
        DetectPlayer();
        _currentState?.OnUpdate();

        if (_player == null || Skill == null) return;
        if (_isUsingSkill) return;

        bool inRange = IsPlayerInSkillRange();
        bool skillReady = Skill.IsReady(this);

        if (!_isAttacking && inRange && skillReady)
            StartAttack();
        else if (_isAttacking && (!inRange || !skillReady))
            StopAttack();
    }

    private void FixedUpdate()
    {
        if (_player == null) return;

        UpdateDirection();
        UpdateFovDirection();
        _currentState?.OnFixedUpdate();
    }

    private void DetectPlayer()
    {
        bool playerFound = false;

    foreach (var obj in _aiFov.GetDetectedObjects())
    {
        if (obj.TryGetComponent<PlayerStatus>(out _))
        {
            _player = obj.transform;
            _destinationSetter.target = _player;
            playerFound = true;
            break;
        }
    }

    if (!_foundPlayer && !playerFound)
    {
        _destinationSetter.target = null;
        StopMoving();
    }
}

    public virtual void UpdateFovDirection()
    {
        if (_player == null || _aiFov == null) return;
        float angle = transform.localScale.x < 0 ? 0f : 180f;
        _aiFov.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    public virtual void UpdateDirection()
    {
        if (_player == null || _isUsingSkill) return;

        float direction = _player.position.x - transform.position.x;
        Vector3 scale = transform.localScale;
        scale.x = direction > 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void ChasePlayer() => _aiPath.canMove = true;
    public void StopMoving() => _aiPath.canMove = false;

    public void ChangeState(AI_IState newState)
    {
        if (_currentState?.GetType() == newState.GetType()) return;

        _currentState?.OnExit();
        _currentState = newState;
        _currentState?.OnEnter();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (Health > 0)
            _damageEffect.CallDamageFlash();
        
        if (!_isUsingSkill && _currentState is not AI_StateDie)
            _player = FindObjectOfType<PlayerStatus>().transform;
            ForceDetectTarget(_player);

        if (Health <= 0f && _currentState is not AI_StateDie)
            ChangeState(new AI_StateDie(this));
    }


    public bool IsPlayerDetected() =>
        _foundPlayer || (_player != null && _aiFov.GetDetectedObjects().Contains(_player.gameObject));

    public virtual bool IsPlayerInSkillRange() =>
        _player != null && Vector2.Distance(transform.position, _player.position) <= SkillRange;

    public virtual void ForceDetectTarget(Transform player)
    {
        _player = player;
        _destinationSetter.target = player;
        _foundPlayer = true;
        _aiPath.destination = player.position;
        _aiPath.canSearch = true;
        _aiPath.SearchPath();
        _aiPath.canMove = true;
        ChangeState(new AI_StateWalk(this));
    }

    public bool IsAttacking() => _isAttacking;

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
        ChasePlayer();
        ChangeState(new AI_StateIdle(this));
    }

    public void DoSkill()
    {
        if (Skill == null || !Skill.IsReady(this)) return;

        _isUsingSkill = true;
        StopMoving();

        Skill.StartSkill(this, () =>
        {
            _isUsingSkill = false;
            ChasePlayer();
        });
    }

    private void ConfigurePathfinding()
    {
        _aiPath.radius = 0.41f;
        _aiPath.height = 0.01f;
        _aiPath.maxSpeed = MoveSpeed;
        _aiPath.pickNextWaypointDist = 1.2f;
        _aiPath.orientation = OrientationMode.YAxisForward;
        _aiPath.enableRotation = false;
        _aiPath.gravity = Vector3.zero;
    }
}
