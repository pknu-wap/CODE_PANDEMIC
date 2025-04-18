using System.Diagnostics;

public class AI_StateWalk : AI_IState
{
    private readonly AI_Controller _controller;

    public AI_StateWalk(AI_Controller controller)
    {
        _controller = controller;
    }

    public void OnEnter()
    {
        _controller.ChasePlayer();
    }

    public void OnUpdate()
    {
        if (!_controller.IsPlayerDetected())
        {
            _controller.ChangeState(new AI_StateIdle(_controller));
            _controller._animator.SetTrigger("Idle");
        }
    }

    public void OnFixedUpdate() { }

    public void OnExit()
    {
        _controller.StopMoving();
    }
}