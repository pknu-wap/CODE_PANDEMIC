public class AI_StateDie : AI_IState
{
    private readonly AI_Controller _controller;
    public AI_StateDie(AI_Controller controller){
        _controller = controller;
    }

    public void OnEnter()
    {
        _controller.Die(); // 실제 죽는 처리
    }

    public void OnUpdate() { }

    public void OnExit() { }
}
