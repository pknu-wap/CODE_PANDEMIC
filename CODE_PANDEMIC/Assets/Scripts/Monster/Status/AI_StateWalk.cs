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
        _controller._animator.SetBool("Walk", true);
    }

    public void OnUpdate()
    {
        if (!_controller.IsPlayerDetected() && !_controller._attackedPlayer)
        {
            _controller.ChangeState(new AI_StateIdle(_controller));
            _controller._animator.SetBool("Walk", false);
        }
    }

    public void OnFixedUpdate() { }

    public void OnExit()
    {
        _controller.StopMoving();
    }
}