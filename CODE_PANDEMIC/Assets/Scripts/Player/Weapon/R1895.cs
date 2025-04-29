using UnityEngine;

public class R1895 : PistolWeaponBase
{
    public float bulletSpeed = 15f;
    public float reloadTime = 2f;
    public float spreadAngle = 0f;
    public int bulletCount = 6;
    public string weaponPrefab = "R1895_0";
    public int bulletID = 1;

    private Animator animator;
    private bool isPickedUp = false;



    void Start()
    {
        weaponName = "R1895";
        damage = 25f;
        fireRate = 0.5f;
        range = 10f;

        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPickedUp) return;

        EquipWeapon equipWeapon = collision.GetComponent<EquipWeapon>();
        if (equipWeapon != null)
        {
            equipWeapon.Equip(this);
            isPickedUp = true;
        }
    }

    public override void Attack()
    {
        Debug.Log("R1895 Attack() called");  // 이 줄 추가

        if (!CanFire()) return;
        SetNextFireTime();

        if (animator != null)
        {
            animator.SetTrigger("Fire");
        }

        if (firePoint == null) return;

        GameObject bulletObject = BulletPool.Instance.GetBullet();
        bulletObject.transform.position = firePoint.position;
        bulletObject.transform.rotation = firePoint.rotation;

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Fire(firePoint.right);
        }

    }

} 
