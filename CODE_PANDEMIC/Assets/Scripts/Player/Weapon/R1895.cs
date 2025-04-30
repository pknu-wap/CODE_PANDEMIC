using UnityEngine;

public class R1895 : PistolWeaponBase
{
    public float bulletSpeed = 15f;
    public float reloadTime = 2f;
    public float spreadAngle = 0f;
    public int bulletCount = 6;
    public int bulletID = 1;

    private Animator animator;
    private bool isPickedUp = false;


    void Start()
    {
        weaponName = "R1895";
        damage = 25f;
        fireRate = 0.05f;
        range = 10f;

        animator = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPickedUp) return;
    }

    public override void Attack()
    {
        if (!CanFire()) return;
        SetNextFireTime();

        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = BulletPool.Instance.GetBullet();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Fire(firePoint.right);
            }

            Debug.Log($"ÃÑ¾Ë »ý¼ºµÊ at {firePoint.position}");
            Debug.Log($"ÃÑ¾Ë ¹ß»çµÊ ¹æÇâ: {firePoint.right}");
        }
    }

    
}
