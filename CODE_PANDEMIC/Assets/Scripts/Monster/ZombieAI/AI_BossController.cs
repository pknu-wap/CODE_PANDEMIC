using UnityEngine;
using Pathfinding;

public class AI_BossController : AI_Controller
{

    protected new AI_IState _currentState;

    protected float _lostTimer = 0f;
    protected float _playerLostDelay = 1f;

    // public virtual void TryUseSkill(System.Action onSkillComplete)
    // {
    //     onSkillComplete?.Invoke();
    // }

    protected override void Start()
    {
        base.Start();
    }

    public override bool Init()
    {
        ConfigurePathfinding();
        ChangeState(new AI_BossIdle(this));
        return true;
    }

    private void Update()
    {
        TryDetectPlayer();
        _currentState?.OnUpdate();

        if (_player == null || Skill == null) return;

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
        UpdateDirection();
        UpdateFovDirection();
        _currentState?.OnFixedUpdate();
    }

    protected void TryDetectPlayer()
    {
        bool found = false;
        foreach (var obj in _aiFov.GetDetectedObjects())
        {
            if (obj.TryGetComponent<PlayerStatus>(out _))
            {
                _player = obj.transform;
                _destinationSetter.target = _player;
                _lostTimer = _playerLostDelay;
                found = true;
                break;
            }
        }

        if (!found)
        {
            if (_lostTimer > 0f)
                _lostTimer -= Time.deltaTime;
            else
            {
                _player = null;
                _destinationSetter.target = null;
                StopMoving();
            }
        }
    }

    public override void UpdateDirection()
    {
        if (_player == null || _isUsingSkill) return;
        Vector3 scale = transform.localScale;
        scale.x = (_player.position.x > transform.position.x) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public override void UpdateFovDirection()
    {
        if (_player == null) return;
        float angle = transform.localScale.x < 0 ? 0f : 180f;
        _aiFov.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected void ConfigurePathfinding()
    {
        _aiPath.canMove = true;
        _aiPath.enableRotation = false;
        _aiPath.gravity = new Vector3(0, 0, 0);
        _aiPath.orientation = OrientationMode.YAxisForward;
        _aiPath.pickNextWaypointDist = 1.2f;
        _aiPath.maxSpeed = MoveSpeed;
    }

    public override bool IsPlayerInSkillRange()
    {
        return _player != null && Vector2.Distance(transform.position, _player.position) <= 10f;
    }

    public new void ChangeState(AI_IState newState)
    {
        if (_currentState != null && _currentState.GetType() == newState.GetType()) return;

        _currentState?.OnExit();
        _currentState = newState;
        _currentState?.OnEnter();
    }

    public new void StartAttack()
    {
        if (_isAttacking || _player == null) return;

        _isAttacking = true;
        StopMoving();
        ChangeState(new AI_BossAttack(this));
    }

    public new void StopAttack()
    {
        if (!_isAttacking) return;

        _isAttacking = false;
        _aiPath.canMove = true;
        ChangeState(new AI_BossIdle(this));
    }
}
