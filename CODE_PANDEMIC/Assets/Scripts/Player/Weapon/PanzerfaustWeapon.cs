using UnityEngine;

public class PanzerFaustWeapon : WeaponBase
{
    [SerializeField] private GameObject PanzerfaustProjectilePrefab;
    [SerializeField] private GameObject firePoint;

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

        if (_currentAmmo <= 0) Reload();

        Managers.Event.InvokeEvent("BulletUpdated", _currentAmmo);
        if (_animator != null) _animator.SetBool("Fire", true);

        if (firePoint != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - firePoint.transform.position;
            direction.z = 0f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (weaponSpriteRenderer != null)
                weaponSpriteRenderer.flipY = (mousePos.x < firePoint.transform.position.x);
        }

        if (PanzerfaustProjectilePrefab != null && firePoint != null)
        {
            GameObject bulletObject = BulletPool.Instance.GetBullet();
            bulletObject.transform.position = firePoint.transform.position;
            bulletObject.transform.rotation = firePoint.transform.rotation;

            PanzerfaustProjectile proj = bulletObject.GetComponent<PanzerfaustProjectile>();
            if (proj != null)
            {
                proj.Init(
                    _weaponData.Damage,
                    _weaponData.BulletSpeed,
                    _weaponData.Range,
                    owner,
                    firePoint.transform.right
                );
            }
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
