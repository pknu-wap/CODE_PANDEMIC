using UnityEngine;

public class AI_BossSweep : AI_SweepSkill
{
    public void SetSettings(SweepSkillData settings, LayerMask targetLayer, AI_HospitalBoss controller)
    {
        _settings = settings;
        _targetLayer = targetLayer;
        _bossController = controller;
    }

    private AI_HospitalBoss _bossController;

    protected override float GetDamageMultiplier()
    {
        return _bossController != null && _bossController.IsBerserk
            ? 0.7f
            : 0.5f;
    }
    protected override void DoSweepAttack(Vector2 forward)
    {
        _bossController._animator.SetTrigger("Sweep");
        _settings.Range = _bossController.IsBerserk ? 6f : 4f;
        base.DoSweepAttack(forward);
    }
}
