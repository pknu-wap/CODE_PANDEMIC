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
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = (mousePos - firePoint.position);
            dir.z = 0f;
            dir.Normalize();

            rb.velocity = dir * fireForce;
        }

        var projectile = proj.GetComponent<PanzerfaustProjectile>();
        if (projectile != null)
        {
            projectile.SetDamage(_weaponData.Damage);
            projectile.SetRange(_weaponData.Range); // Range 전달
        }
    }


}
