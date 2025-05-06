using UnityEngine;

public class WideSpreadWeaponBase : WeaponBase
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;

    public override void Attack(PlayerController owner)
    {
        if (!CanFire()) return;

       
        SetNextFireTime();

        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
