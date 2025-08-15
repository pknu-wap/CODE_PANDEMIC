using Pathfinding;
public interface AI_IState
{
    void OnEnter(AI_Controller controller);
    void OnUpdate();
    void OnExit();
    void OnFixedUpdate();
}
public abstract class AI_StateBase : AI_IState
{
    protected AI_Controller _controller;
    protected AI_Movement _movement;
    protected AI_Detection _detection;
    protected AI_Combat _combat;

    public virtual void OnEnter(AI_Controller controller)
    {
        _controller = controller;
        _movement = controller.GetComponent<AI_Movement>();
        _detection = controller.GetComponent<AI_Detection>();
        _combat = controller.GetComponent<AI_Combat>();
    }

    public virtual void OnExit() { }
    public abstract void OnUpdate();
    public virtual void OnFixedUpdate() { }
}
