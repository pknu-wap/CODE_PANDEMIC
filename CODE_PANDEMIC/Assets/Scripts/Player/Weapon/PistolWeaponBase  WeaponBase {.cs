using UnityEngine;

public class PistolWeaponBase : WeaponBase
{
    [SerializeField]
     GameObject bulletPrefab;
    [SerializeField]
    private GameObject firePoint;

    private Animator _animator;
    private bool isPickedUp = false;


    void Start()
    {
        _animator = GetComponent<Animator>();
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
            GameObject bulletObject = BulletPool.Instance.GetBullet();
            bulletObject.transform.position = firePoint.transform.position;
            bulletObject.transform.rotation = firePoint.transform.rotation;

            Bullet bullet = bulletObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetInfo(_weaponData.Damage);
                bullet.Fire(firePoint.transform.right);
            }


            Debug.Log($"ÃÑ¾Ë ¹ß»çµÊ ¹æÇâ: {firePoint.transform.right}");
        }

    }
}