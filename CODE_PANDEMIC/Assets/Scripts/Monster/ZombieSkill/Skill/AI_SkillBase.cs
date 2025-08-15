using UnityEngine;

public abstract class AI_SkillBase : ISkillBehavior
{
    protected AI_Controller _controller;
    protected AI_Movement _movement;
    protected AI_Detection _detection;
    protected AI_Combat _combat;
    protected LayerMask _targetLayer;

    public virtual bool IsReady(AI_Controller controller) { return true; }
    public virtual void StartSkill(AI_Controller controller, System.Action onSkillComplete) { }
    public virtual void StopSkill() { }
    public void SetController(AI_Controller controller) { _controller = controller; }
    public virtual void SetSettings(object settings, LayerMask targetLayer, AI_Controller controller) 
    {
        _targetLayer = targetLayer;
        _controller = controller;
        _movement = controller.GetComponent<AI_Movement>();
        _detection = controller.GetComponent<AI_Detection>();
        _combat = controller.GetComponent<AI_Combat>();
    }
}
