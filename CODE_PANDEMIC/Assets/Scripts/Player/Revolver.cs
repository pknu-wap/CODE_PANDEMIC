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

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanFire())
        {
            Attack();
        }
    }

    public override void Attack()
    {
        if (bulletPrefab == null || firePoint == null) return;

        SetNextFireTime();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = firePoint.up * bulletSpeed;
        }
    }

    public override void Reload()
    {
        Debug.Log(weaponName + " is reloading with revolver-specific animation.");
    }
}

