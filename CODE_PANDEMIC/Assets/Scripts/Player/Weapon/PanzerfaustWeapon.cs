using UnityEngine;

public class PanzerfaustWeapon : WeaponBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireForce = 10f;

    public override void Attack(PlayerController owner)
    {
        if (!CanFire()) return;
        SetNextFireTime();

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * fireForce;
        }

        var projectile = proj.GetComponent<PanzerfaustProjectile>();
        if (projectile != null)
        {
            projectile.SetDamage(_weaponData.Damage);
        }
    }
}
