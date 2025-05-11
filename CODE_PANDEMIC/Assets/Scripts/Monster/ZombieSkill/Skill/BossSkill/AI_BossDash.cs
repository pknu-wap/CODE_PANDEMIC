using UnityEngine;

public class AI_BossDash : AI_DashSkill
{
    private AI_HospitalBoss _boss;

    public void SetController(AI_BossController controller)
    {
        base.SetController(controller);
        _boss = controller as AI_HospitalBoss;
    }

    protected override float Cooldown => _boss?.DashCooldown ?? 20f;
    protected override float DashDuration => _boss?.DashDuration ?? 0.3f;
    protected override float DashSpeed => (_boss != null && _boss.DashDuration > 0f)
        ? _boss.DashRange / _boss.DashDuration
        : 40f;

    protected override float DashRange => _boss?.DashRange ?? 12f;
    protected override float DashWidth => _boss?.DashWidth ?? 5f;
}
