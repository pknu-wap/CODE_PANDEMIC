using UnityEngine;
using System.Collections;

public class WideSpreadWeaponBase : WeaponBase
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject firePoint;

    [SerializeField] protected int pelletCount = 6;       // ÃÑ¾Ë °³¼ö
    [SerializeField] protected float spreadAngle = 15f;   // ÆÛÁü °¢µµ
    [SerializeField] protected float fireForce = 10f;     // ¹ß»ç ¼Óµµ

    private Animator _animator;

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

        if (bulletPrefab != null && firePoint != null)
        {
            for (int i = 0; i < pelletCount; i++)
            {
                float angleOffset = Random.Range(-spreadAngle, spreadAngle);
                Quaternion rotation = Quaternion.Euler(0f, 0f, angleOffset) * firePoint.transform.rotation;

                GameObject bulletObject = BulletPool.Instance.GetBullet();
                bulletObject.transform.position = firePoint.transform.position;
                bulletObject.transform.rotation = rotation;

                Bullet bullet = bulletObject.GetComponent<Bullet>();
                if (bullet != null)
                {
                    bullet.SetInfo(_weaponData.Damage);

                    Vector2 fireDirection = rotation * Vector2.right;
                    bullet.Fire(fireDirection.normalized * fireForce);
                }

                Debug.Log($"ÃÑ¾Ë ¹ß»çµÊ ¹æÇâ: {rotation * Vector2.right}");
            }
        }

        StartCoroutine(ResetFireBool());
    }

    private IEnumerator ResetFireBool()
    {
        yield return new WaitForSeconds(0.1f);
        if (_animator != null)
        {
            _animator.SetBool("Fire", false);
        }
    }
}
