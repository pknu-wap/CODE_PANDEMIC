using UnityEngine;

public class AI_BossThrow : AI_ThrowSkill
{
    private AI_HospitalBoss _bossController;

    public void SetController(AI_HospitalBoss controller)
    {
        _bossController = controller;
    }

    public void SetSettings(ThrowSkillData settings, LayerMask targetLayer, AI_HospitalBoss controller)
    {
        _settings = settings;
        _targetLayer = targetLayer;
        _bossController = controller;
    }

    protected override void ThrowSyringe(Vector2 direction)
    {
        if (_bossController == null || _bossController._syringePrefab == null) return;

        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float[] angles = _bossController.IsBerserk
            ? new float[] { +45f, +15f, -15f, -45f }
            : new float[] { +45f, 0f, -45f };

        foreach (float offset in angles)
        {
            float angle = baseAngle + offset;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
            Vector2 shootPos = (Vector2)_bossController.transform.position + dir;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject syringe = Object.Instantiate(_bossController._syringePrefab, shootPos, rotation);
            syringe.transform.localScale = new Vector3(
                Mathf.Abs(syringe.transform.localScale.x),
                syringe.transform.localScale.y,
                syringe.transform.localScale.z
            );

            if (syringe.TryGetComponent(out Projectile projectile))
            {
                projectile.SetOwner(_controller);
                projectile.MaxDistance = _settings.Range;
            }

            if (syringe.TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = dir * _settings.SyringeSpeed;
            }
        }
    }
}
