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
        _controller.StartAttack(); // 이곳에서 데미지 코루틴 시작
    }

    public void OnUpdate()
    {
        // 만약 플레이어가 충돌 영역에서 벗어나면 Walk 상태로 전환
        if (!_controller._playerInTrigger)
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
