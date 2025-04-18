using System.Collections;
using UnityEngine;

public class AI_StateAttack : AI_IState
{
    private bool _isSkillPlaying;
    private float _chargeDelay = 1f;
    private readonly AI_Controller _controller;

    public AI_StateAttack(AI_Controller controller) 
    { 
        _controller = controller;
    }

    public virtual void OnEnter()
    {
        _controller.StopMoving();
        _isSkillPlaying = true;
        _controller.StartCoroutine(ChargeAndExecuteSkill());
    }

    public virtual void OnUpdate()
    {
        if (_isSkillPlaying)
            return;
    }

    public virtual void OnFixedUpdate() { }

    public virtual void OnExit()
    {
        if (_controller.Skill != null)
            _controller.Skill.StopSkill();
        _isSkillPlaying = false;
    }

    private IEnumerator ChargeAndExecuteSkill()
    {
        yield return new WaitForSeconds(_chargeDelay);

        if (_controller.Skill != null && _controller.Skill.IsReady(_controller))
        {
            _controller.Skill.StartSkill(_controller, OnSkillComplete);
        }
        else
        {
            OnSkillComplete();
        }
    }

    private void OnSkillComplete()
    {
        _isSkillPlaying = false;
        _controller.ChangeState(new AI_StateIdle(_controller));
        _controller._animator.SetTrigger("Idle");
    }
}