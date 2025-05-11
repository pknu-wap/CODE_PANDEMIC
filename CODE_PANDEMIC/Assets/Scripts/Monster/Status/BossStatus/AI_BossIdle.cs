using UnityEngine;
public class AI_BossIdle : AI_StateBase
{
    private float _idleTime = 1f;
    private float _elapsedTime = 0f;
    private AI_BossController _bossController;

    public AI_BossIdle(AI_Controller controller) : base(controller)
    {
        _bossController = controller as AI_BossController;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _elapsedTime = 0f;
        _bossController._animator.SetTrigger("Idle");
    }

    public override void OnUpdate()
    {
        _elapsedTime += Time.deltaTime;

        if (_bossController.IsPlayerInSkillRange())
        {
            _bossController.ChangeState(new AI_BossAttack(_bossController));
        }
        else if (_elapsedTime >= _idleTime)
        {
            _bossController.ChangeState(new AI_BossWalk(_bossController));
        }
    }
}
