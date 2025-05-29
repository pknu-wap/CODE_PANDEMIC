using UnityEngine;

public class WideSpreadWeaponBase : WeaponBase
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

        if (firePoint != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - firePoint.transform.position;
            direction.z = 0f;

            float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.transform.rotation = Quaternion.Euler(0f, 0f, baseAngle);

            // 여기서부터 샷건(퍼짐) 로직
            int bulletCount = _weaponData.BulletCount;   // 예: 8
            float spreadAngle = _weaponData.SpreadAngle; // 예: 25도

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
                //Debug.Log($"[WideSpreadWeaponBase] {i + 1}번째 총알, 방향: {bulletObject.transform.right}");
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