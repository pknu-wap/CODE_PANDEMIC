using UnityEngine;

public class Revolver : WeaponBase
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    public float bulletSpeed = 15f;

    void Start()
    {
        weaponName = "Revolver";
        damage = 25f;
        fireRate = 2f;
        range = 10f;
    }

    public override void Attack()
    {
        Debug.Log("Revolver Attack() called");

        if (bulletPrefab == null)
        {
            Debug.LogWarning("bulletPrefab is not assigned.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogWarning("firePoint is not assigned.");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = firePoint.up * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet prefab has no Rigidbody2D.");
        }
    }
}
