using System.Diagnostics;
public class AI_StateIdle : AI_IState
{
    private readonly AI_Controller _controller;

    public AI_StateIdle(AI_Controller controller)
    {
        _controller = controller;
    }

    public void OnEnter()
    {
        _controller._animator.SetBool("Walk", false);
        _controller.StopMoving();
    }

    public void OnUpdate()
    {
        if (_controller.IsPlayerDetected())
        {
            _controller.ChangeState(new AI_StateWalk(_controller));
            _controller._animator.SetBool("Walk", true);
        }
    }

    public void OnFixedUpdate() { }

    public void OnExit() { }
}
