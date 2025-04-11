using UnityEngine;

public class Mes : WideSpreadWeaphonBase
{
    [SerializeField] private float fireForce = 15f;

    public override void Attack()
    {
        if (!CanFire()) return;

        Debug.Log(weaponName + " fired a mes!");
        SetNextFireTime();

        if (bulletPrefab != null && firePoint != null)
        {
            GameObject dart = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = dart.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = firePoint.right * fireForce;
            }
        }
    }
}
