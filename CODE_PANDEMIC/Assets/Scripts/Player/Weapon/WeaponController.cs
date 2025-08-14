using UnityEngine;
using System.Collections;

public class WeaponController : WeaponBase
{
    [Header("Weapon Settings")]
    [Tooltip("한 번에 발사되는 총알 수. 1이면 권총/근거리, 1보다 크면 산탄총/광범위로 동작합니다.")]
    [SerializeField] private int bulletCount = 1;

    [SerializeField] private float spreadAngle = 25f;

    [Header("Dependencies")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private Animator _animator;
    private bool _isPickedUp = false;
    private float _damage;
    private float _fireRate;


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Attack(PlayerController owner)
    {
        if (!CanFire(owner)) return;

        SetNextFireTime(owner);
        _currentBullet--;
        Managers.Event.InvokeEvent("BulletUpdated", _currentBullet);

        if (_currentBullet <= 0)
        {
            Reload();
        }

        // 애니메이션 재생
        if (_animator != null)
        {
            _animator.SetBool("Fire", true);
        }

        // 총알 발사 로직 선택
        if (bulletCount > 1)
        {
            ShootWideSpread();
        }
        else
        {
            ShootSingleBullet();
        }

        StartCoroutine(ResetFireBool());
    }

    public void SetWeaponData(WeaponData data)
    {
        _weaponData = data;
        _damage = data.Damage;
        _fireRate = data.FireRate;
        int bulletCount = _weaponData.BulletCount;   // 예: 8
        float spreadAngle = _weaponData.SpreadAngle; // 예: 25도
    }


    private void ShootSingleBullet()
    {
        if (firePoint == null) return;

        // Pistol의 마우스 방향 회전 로직
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - firePoint.transform.position;
        direction.z = 0f;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 총알 생성 및 초기화
        GameObject bulletObject = BulletPool.Instance.GetBullet();
        bulletObject.transform.position = firePoint.transform.position;
        bulletObject.transform.rotation = firePoint.transform.rotation;

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetInfo(_weaponData.Damage);
            bullet.Fire(firePoint.transform.right);
        }
    }


    private void ShootWideSpread()
    {
        if (firePoint == null) return;

        // 마우스 방향으로 회전
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - firePoint.transform.position;
        direction.z = 0f;
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.transform.rotation = Quaternion.Euler(0f, 0f, baseAngle);

        // 산탄총 총알 발사
        float startAngle = baseAngle - (spreadAngle * (bulletCount - 1) / 2f);
        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = startAngle + spreadAngle * i;
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, currentAngle);

            GameObject bulletObject = BulletPool.Instance.GetBullet();
            bulletObject.transform.position = firePoint.transform.position;
            bulletObject.transform.rotation = bulletRotation;

            Bullet bullet = bulletObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetInfo(_weaponData.Damage);
                bullet.Fire(bulletObject.transform.right);
            }
        }
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