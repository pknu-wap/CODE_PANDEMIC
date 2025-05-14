using System.Collections;
using UnityEngine;

public class AI_StateAttack : AI_IState
{
    private float _chargeDelay = 1f;
    private readonly AI_Controller _controller;

    public AI_StateAttack(AI_Controller controller) 
    { 
        _controller = controller;
    }

    public virtual void OnEnter()
    {
        _controller._animator.SetBool("Attack" , true);
        _controller.StopMoving();
        _controller._isUsingSkill = true;
        _controller.StartCoroutine(ChargeAndExecuteSkill());
    }

    public virtual void OnUpdate()
    {
        if (_controller._isUsingSkill)
            return;
    }

    public virtual void OnFixedUpdate() { }

    public virtual void OnExit()
    {
        if (_controller.Skill != null)
            _controller.Skill.StopSkill();
        _controller._isUsingSkill = false;
        _controller.StopAttack();
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
        _controller._isUsingSkill = false;
        _controller.ChangeState(new AI_StateWalk(_controller));
        _controller._animator.SetBool("Attack" , false);
        _controller.StopAttack();
    }
}