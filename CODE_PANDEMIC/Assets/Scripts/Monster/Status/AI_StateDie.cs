using UnityEngine;
public class AI_StateDie : AI_IState
{
    private readonly AI_Controller _controller;
    public AI_StateDie(AI_Controller controller){
        _controller = controller;
    }

    public void OnEnter()
    {
        _controller._animator.SetBool("Walk", false);
        _controller._animator.SetBool("Attack", false);
        _controller._animator.SetTrigger("Die");
        _controller.Die(); // 실제 죽는 처리
    }

    public void OnUpdate() { }

    public void OnExit() { }
}
