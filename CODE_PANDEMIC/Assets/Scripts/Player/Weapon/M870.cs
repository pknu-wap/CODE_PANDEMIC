using UnityEngine;

public class M870 : WideSpreadWeaponBase
{
    [SerializeField] private int pelletCount = 6;
    [SerializeField] private float spreadAngle = 15f;
    [SerializeField] private float fireForce = 10f;

    public override void Attack(PlayerController owner)
    {
        if (!CanFire()) return;

       
        SetNextFireTime();

        for (int i = 0; i < pelletCount; i++)
        {
            float angleOffset = Random.Range(-spreadAngle, spreadAngle);
            Quaternion rotation = Quaternion.Euler(0f, 0f, angleOffset) * firePoint.rotation;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                Vector2 fireDirection = rotation * Vector2.right;
                bulletScript.Fire(fireDirection.normalized * fireForce);
            }
        }
    }
}
