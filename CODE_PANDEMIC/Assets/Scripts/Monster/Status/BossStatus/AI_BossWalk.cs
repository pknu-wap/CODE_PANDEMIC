using UnityEngine;

public class AI_BossWalk : AI_StateBase
{
    private AI_BossController _bossController;
    public AI_BossWalk(AI_Controller controller) : base(controller)
    {
        _bossController = controller as AI_BossController;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _bossController._aiPath.canMove = true;
        _bossController._animator.SetTrigger("Walk");
    }

    public override void OnUpdate()
    {
        if (_bossController.IsPlayerInSkillRange())
        {
            _bossController.ChangeState(new AI_BossAttack(_bossController));
        }
    }
}
