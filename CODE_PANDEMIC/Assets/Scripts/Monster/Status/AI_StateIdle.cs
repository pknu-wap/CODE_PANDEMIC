public class AI_StateIdle : AI_StateBase
{
    public override void OnEnter(AI_Controller controller)
    {
        base.OnEnter(controller);
        _controller._animator.SetBool("Walk", false);
        _movement.StopMoving();
    }

    public override void OnUpdate()
    {
    }
}
