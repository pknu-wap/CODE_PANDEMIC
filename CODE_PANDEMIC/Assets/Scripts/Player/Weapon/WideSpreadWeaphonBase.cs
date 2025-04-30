using UnityEngine;

public class WideSpreadWeaphonBase : WeaponBase
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;

    public override void Attack()
    {
        if (!CanFire()) return;

       
        SetNextFireTime();

        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
