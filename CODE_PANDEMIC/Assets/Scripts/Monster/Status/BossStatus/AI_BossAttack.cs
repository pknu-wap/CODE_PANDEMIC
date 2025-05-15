using UnityEngine;

public class AI_BossAttack : AI_StateBase
{
    private bool _isSkillActive;
    private AI_BossController _bossController;

    public AI_BossAttack(AI_Controller controller) : base(controller)
    {
        _bossController = controller as AI_BossController;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        _isSkillActive = true;
        _bossController._animator.SetTrigger("Attack");

        _bossController.TryUseSkill(() =>
        {
            _isSkillActive = false;
        });
    }

    public override void OnUpdate()
    {
        if (!_isSkillActive)
        {
            _bossController.ChangeState(new AI_BossIdle(_bossController));
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        _bossController._aiPath.canMove = true;
    }
}
