using UnityEngine;
public class AI_StateDie : AI_StateBase
{
    public override void OnEnter(AI_Controller controller)
    {
        base.OnEnter(controller);
        _controller._animator.SetTrigger("Die");
        _controller._animator.SetBool("Walk", false);
        _controller._animator.SetBool("Attack", false);
        _controller.Die();
    }

    public override void OnUpdate() { }
}
