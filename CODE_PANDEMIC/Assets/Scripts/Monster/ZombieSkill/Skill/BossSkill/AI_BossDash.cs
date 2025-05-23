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
}
