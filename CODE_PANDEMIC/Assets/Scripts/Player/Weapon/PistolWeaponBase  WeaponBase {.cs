using UnityEngine;

public class PistolWeaponBase : WeaponBase
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject firePoint;

    private Animator _animator;
    private bool isPickedUp = false;


    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Attack(PlayerController owner)
    {
        if (!CanFire()) return;
        SetNextFireTime();

        if (_animator != null)
        {
            _animator.SetBool("Fire", true);
        }

        if (firePoint != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - firePoint.transform.position;
            direction.z = 0f;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

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

            Debug.Log($"총알 발사됨 방향: {firePoint.transform.right}");
        }

        StartCoroutine(ResetFireBool());
    }


    private System.Collections.IEnumerator ResetFireBool()
    {
        yield return new WaitForSeconds(0.1f);
        if (_animator != null)
        {
            _animator.SetBool("Fire", false);
        }
    }
}