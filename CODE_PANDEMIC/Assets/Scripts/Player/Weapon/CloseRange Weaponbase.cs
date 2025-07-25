using UnityEngine;

public class CloseRangeWeaponBase : WeaponBase
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject firePoint;

    private Animator _animator;
    private bool isPickedUp = false;
    public GameObject originPrefab;
    void Start()
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

        if (_animator != null)
        {
            _animator.SetBool("Fire", true);
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