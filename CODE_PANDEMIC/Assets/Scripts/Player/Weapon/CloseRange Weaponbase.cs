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
            GameObject bulletObject = BulletPool.Instance.GetBullet();
            bulletObject.transform.position = firePoint.transform.position;
            bulletObject.transform.rotation = firePoint.transform.rotation;

            Bullet bullet = bulletObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetInfo(_weaponData.Damage,owner);
                bullet.Fire(firePoint.transform.right);
            }

            Debug.Log($"ÃÑ¾Ë ¹ß»çµÊ ¹æÇâ: {firePoint.transform.right}");
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

    protected void ApplyDamageWithKnockback(Collider2D target, int damage)//³Ë¹é ±¸Çö
    {
        AI_Base enemy = target.GetComponent<AI_Base>();
        if (enemy != null)
        {
            Vector3 knockbackDir = (enemy.transform.position - transform.position).normalized;
            enemy.TakeDamage(damage, knockbackDir);
        }
    }
}