public class AI_StateAttack : AI_IState

{
    private readonly AI_Controller _controller;

    public AI_StateAttack(AI_Controller controller)
    {
        _controller = controller;
    }

    public void OnEnter()
    {
        _controller.StopMoving();
        _controller.StartAttack();
    }

    public void OnUpdate()
    {
        if (!_controller.IsPlayerDetected())
        {
            _controller.ChangeState(new AI_StateWalk(_controller));
        }
    }

    public void OnFixedUpdate() { }

    public void OnExit()
    {
        _controller.StopAttack();
    }
}
