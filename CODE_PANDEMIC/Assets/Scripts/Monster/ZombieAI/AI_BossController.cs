using UnityEngine;
using Pathfinding;

public class AI_BossController : AI_Controller
{
    protected new AI_IState _currentState;

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
        ChangeState(new AI_BossIdle(this));
        return true;
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
