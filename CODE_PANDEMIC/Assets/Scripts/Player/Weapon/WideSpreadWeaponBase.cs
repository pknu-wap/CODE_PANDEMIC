using UnityEngine;
using System.Collections;

public class WideSpreadWeaponBase : WeaponBase
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject firePoint;

    [SerializeField] protected int pelletCount = 6;       // 총알 개수
    [SerializeField] protected float spreadAngle = 15f;   // 퍼짐 각도
    [SerializeField] protected float fireForce = 10f;     // 발사 속도

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Attack(PlayerController owner)
    {
        if (!CanFire()) return;
        SetNextFireTime();
        _currentAmmo--;

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

                Debug.Log($"총알 발사됨 방향: {rotation * Vector2.right}");
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
