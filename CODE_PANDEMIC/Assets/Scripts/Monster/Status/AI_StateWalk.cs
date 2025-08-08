public class AI_StateWalk : AI_StateBase
{
    public override void OnEnter(AI_Controller controller)
    {
        base.OnEnter(controller);
        _movement.ChasePlayer();
        _controller._animator.SetBool("Walk", true);
    }

    public override void OnUpdate()
    {
        if (!_detection.IsPlayerDetected)
        {
            _controller.ChangeState<AI_StateIdle>();
        }
    }

    public override void OnExit()
    {
        _movement.StopMoving();
        _controller._animator.SetBool("Walk", false);
    }
}
