using System.Collections;
using UnityEngine;

public class AI_StateAttack : AI_StateBase
{
    public override void OnEnter(AI_Controller controller)
    {
        base.OnEnter(controller);
        _controller._animator.SetBool("Attack", true);
        _movement.StopMoving();
        _movement.UpdateDirection(_detection.Player);
        _movement._isUsingSkill = true;
        
        _controller.StartCoroutine(ChargeAndExecuteSkill());
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
        _combat.StopAttack(null);
    }

    private IEnumerator ChargeAndExecuteSkill()
    {
        if (_combat.Skill.IsReady(_controller))
        {
            _combat.Skill.StartSkill(_controller, OnSkillComplete);
        }
        else
        {
            OnSkillComplete();
        }
        yield return null; 
    }

    private void OnSkillComplete()
    {
        _movement._isUsingSkill = false;
        _controller._animator.SetBool("Attack", false);
        _controller.ChangeState<AI_StateWalk>();
    }
}
