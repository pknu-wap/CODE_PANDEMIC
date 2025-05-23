public class AI_BossDie : AI_IState
{
    private readonly AI_BossController _controller;
    public AI_BossDie(AI_BossController controller){
        _controller = controller;
    }

    public void OnEnter()
    {
        _controller.Die(); // 실제 죽는 처리
    }

    public void OnUpdate() { }

    public void OnExit() { }
}
