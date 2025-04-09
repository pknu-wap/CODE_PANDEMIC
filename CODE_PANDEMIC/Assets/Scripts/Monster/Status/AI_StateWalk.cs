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
        // 아무것도 안 해도 됨 — 충돌로 상태 바뀜
        if (!_controller.IsPlayerDetected())
        {
            _controller.ChangeState(new AI_StateIdle(_controller));
        }
    }

    public void OnFixedUpdate() { }

    public void OnExit()
    {
        _controller.StopMoving();
    }
}