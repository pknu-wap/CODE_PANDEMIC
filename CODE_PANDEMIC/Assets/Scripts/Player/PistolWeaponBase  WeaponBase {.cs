using UnityEngine;

public class PistolWeaponBase : WeaponBase
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

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
