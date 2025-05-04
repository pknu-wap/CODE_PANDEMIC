using UnityEngine;

public class Mes : WeaponBase
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject firePoint;

    private bool isPickedUp = false;

    public override void Attack()
    {
        if (!CanFire()) return;
        SetNextFireTime();


        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bulletObject = BulletPool.Instance.GetBullet();
            bulletObject.transform.position = firePoint.transform.position;
            bulletObject.transform.rotation = firePoint.transform.rotation;

            Bullet bullet = bulletObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetInfo(_weaponData.Damage);
                bullet.Fire(firePoint.transform.right);
            }

            Debug.Log($"Mes πﬂªÁµ  πÊ«‚: {firePoint.transform.right}");
        }

    }

}