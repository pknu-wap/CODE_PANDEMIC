using UnityEngine;

public class R1895 : PistolWeaponBase
{
    public float bulletSpeed = 15f;

    void Start()
    {
        weaponName = "R1895";
        damage = 25f;
        fireRate = 0.5f;
        range = 10f;
    }

    public override void Attack()
    {
        if (!CanFire()) return;
        SetNextFireTime();

        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = firePoint.up * bulletSpeed;
        }
    }
}
