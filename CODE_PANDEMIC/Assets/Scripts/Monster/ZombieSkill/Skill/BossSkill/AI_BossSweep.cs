using UnityEngine;

public class AI_BossSweep : AI_SweepSkill
{
    private AI_HospitalBoss _bossController;
    public void SetController(AI_BossController controller)
    {
        _bossController = controller as AI_HospitalBoss;
    }
    
    protected override float Cooldown => _bossController.SweepCooldown;
    protected override float ChargeDelay => _bossController.SkillChargeDelay;
    protected override int SweepCount => _bossController.SweepCount;

    protected override float SweepInterval => _bossController.SweepInterval;
    protected override float SweepRange => _bossController.SweepRange;
    protected override float SweepAngle => _bossController.SweepAngle;
    protected override LayerMask TargetLayer => LayerMask.GetMask("Player");
    protected override void DoSweepAttack(Vector2 forward)
    {
        Vector2 origin = _controller.transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, SweepRange, TargetLayer);

        foreach (var hit in hits)
        {
            Vector2 toTarget = ((Vector2)hit.transform.position - origin).normalized;
            if (Vector2.Angle(forward, toTarget) <= SweepAngle * 0.5f)
            {
                float damage = _bossController?.IsBerserk == true ? _controller.AiDamage * 0.7f : _controller.AiDamage * 0.5f;

                if (hit.TryGetComponent<PlayerStatus>(out var player))
                {
                    Debug.Log($"[AI_BossSweep] hit {player.gameObject.name} for {damage} {SweepInterval} damage.");
                    // player.OnDamaged(_controller.gameObject, damage);
                }
            }
        }
    }
}
