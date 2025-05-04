using UnityEngine;

public class CloseRangeWeaponBase : WeaponBase
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

    public override void Attack()
    {
        if (!CanFire()) return;
        SetNextFireTime();


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

            Debug.Log($"ÃÑ¾Ë ¹ß»çµÊ ¹æÇâ: {firePoint.transform.right}");
        }

        StartCoroutine(ResetFireBool());
    }

    private System.Collections.IEnumerator ResetFireBool()
    {
        yield return new WaitForSeconds(0.05f);
        if (_animator != null)
        {
            _animator.SetBool("Fire", false);
        }
    }
}
