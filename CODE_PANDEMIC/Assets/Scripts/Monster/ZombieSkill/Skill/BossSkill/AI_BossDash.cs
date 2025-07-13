using System;
using System.Collections;
using UnityEngine;

public class AI_BossDash : AI_DashSkill
{
    private AI_HospitalBoss _bossController;
    public void SetSettings(DashSkillData settings, LayerMask targetLayer, AI_HospitalBoss controller)
    {
        _settings = settings;
        _targetLayer = targetLayer;
        _bossController = controller;
    }
    public override void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        _controller = controller;
        _controller._animator.SetTrigger("Dash");
        _baseAnimationDuration = 0.8f;
        base.StartSkill(controller, onSkillComplete);
    }
}
