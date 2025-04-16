using UnityEngine;

public class PistolWeaponBase : WeaponBase
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public override void Attack()
    {
        if (!CanFire()) return;

        Debug.Log(weaponName + " fired a pistol shot!");
        SetNextFireTime();

        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}