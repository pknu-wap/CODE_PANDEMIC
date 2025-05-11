using UnityEngine;

public class AI_BossThrow : AI_ThrowSkill
{
    private AI_HospitalBoss _bossController;
    public void SetController(AI_BossController controller)
    {
        _bossController = controller as AI_HospitalBoss;
    }

    protected override float Cooldown => _bossController.ThrowCooldown;
    protected override float ChargeDelay => _bossController.SkillChargeDelay;

    protected override void ThrowSyringe(Vector2 direction)
{
    if (_bossController == null) return;

    int count = _bossController.IsBerserk ? 4 : 3;
    float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    float[] angles = _bossController.IsBerserk
        ? new float[] { +45f, +15f, -15f, -45f }
        : new float[] { +45f, 0f, -45f };

    foreach (float offset in angles)
    {
        float angle = baseAngle + offset;
        Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

        Vector2 shootPos = (Vector2)_bossController.transform.position + dir * 1.2f;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject syringe = GameObject.Instantiate(_bossController._syringePrefab, shootPos, rotation);

        syringe.transform.localScale = new Vector3(
            Mathf.Abs(syringe.transform.localScale.x),
            syringe.transform.localScale.y,
            syringe.transform.localScale.z
        );
        Projectile projectile = syringe.GetComponent<Projectile>();
        projectile.SetOwner(_controller);
        projectile.MaxDistance = _bossController._syringePrefab.GetComponent<Projectile>().MaxDistance;

        Rigidbody2D rb = syringe.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dir * _bossController.SyringeSpeed;
        }
    }
}



}
