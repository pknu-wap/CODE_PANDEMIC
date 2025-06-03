using UnityEngine;

public class PanzerFaustWeapon : WeaponBase
{
    [SerializeField] private GameObject panzerfaustProjectilePrefab;
    [SerializeField] private Transform firePoint;
    private Animator _animator;
    private bool isPickedUp = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Attack(PlayerController owner)
    {
        if (!CanFire(owner)) return;
        SetNextFireTime(owner);
        _currentBullet--;

        if (_currentBullet <= 0)
            Reload();

        if (firePoint != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - firePoint.position;
            direction.z = 0f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0f, 0f, angle);

            if (weaponSpriteRenderer != null)
                weaponSpriteRenderer.flipY = (mousePos.x < firePoint.position.x);
        }

        if (panzerfaustProjectilePrefab != null && firePoint != null)
        {
            GameObject rocketObject = PanzerfaustPool.Instance.GetRocket();
            if (rocketObject == null) return;

            rocketObject.transform.position = firePoint.position;
            rocketObject.transform.rotation = firePoint.rotation;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            Vector3 direction = mousePos - firePoint.position;

            PanzerfaustProjectile proj = rocketObject.GetComponent<PanzerfaustProjectile>();
            if (proj != null)
            {
                proj.Init(_weaponData.Damage, _weaponData.BulletSpeed, _weaponData.Range, owner, direction);
            }
        }


        Managers.Event.InvokeEvent("BulletUpdated", _currentBullet);
        if (_animator != null)
            _animator.SetBool("Fire", true);

        StartCoroutine(ResetFireBool());
    }

    private System.Collections.IEnumerator ResetFireBool()
    {
        yield return new WaitForSeconds(0.1f);
        if (_animator != null)
            _animator.SetBool("Fire", false);
    }
}
