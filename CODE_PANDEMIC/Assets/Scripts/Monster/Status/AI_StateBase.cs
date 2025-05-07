using Pathfinding;
public interface AI_IState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
    void OnFixedUpdate() {}
}
public abstract class AI_StateBase : AI_IState
{
    protected AI_Controller _controller;
    public AI_StateBase(AI_Controller controller)
    {
        _controller = controller;
    }

    public virtual void OnEnter() {
        _controller._aiPath.canMove = true;
    }
    public virtual void OnExit() { }

    public abstract void OnUpdate();
}
