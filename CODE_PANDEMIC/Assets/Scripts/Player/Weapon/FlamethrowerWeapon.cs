using UnityEngine;

public class FlamethrowerWeapon : WeaponBase
{
    [SerializeField] private GameObject flameHitboxPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject flameEffectPrefab;
    private GameObject _currentEffect;

    private GameObject _currentFlame;
    private bool _isFiring = false;



    public override void Attack(PlayerController owner)
    {
        Debug.Log("[FlamethrowerWeapon] Attack 호출");
        StartAttack(owner);

        if (firePoint != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - firePoint.transform.position;
            direction.z = 0f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            firePoint.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (weaponSpriteRenderer != null)
            {
                weaponSpriteRenderer.flipY = (mousePos.x < firePoint.transform.position.x);
            }
        }
    }

    public override void StartAttack(PlayerController owner)
    {
        Debug.Log($"[FlamethrowerWeapon] StartAttack 호출, _isFiring={_isFiring}, _isReloading={_isReloading}, WeaponData.Damage={_weaponData?.Damage}");

        if (_isFiring || _isReloading) return;
        _isFiring = true;

        // 불꽃 판정 오브젝트(히트박스) 켜기
        if (_currentFlame == null)
        {
            Debug.Log("[FlamethrowerWeapon] flameHitboxPrefab Instantiate");
            _currentFlame = Instantiate(flameHitboxPrefab, firePoint.position, firePoint.rotation, firePoint);
            FlameHitbox hitbox = _currentFlame.GetComponent<FlameHitbox>();
            Debug.Log($"[FlamethrowerWeapon] FlameHitbox SetInfo 호출 전 WeaponData.Damage={_weaponData?.Damage}");
            hitbox.SetInfo(_weaponData);
        }
        else
        {
            Debug.Log("[FlamethrowerWeapon] flameHitbox Already Exists");
            // **여기도 SetInfo 강제로 다시 해보기**
            FlameHitbox hitbox = _currentFlame.GetComponent<FlameHitbox>();
            hitbox.SetInfo(_weaponData);
        }
        _currentFlame.SetActive(true);

        // 불꽃 이펙트 켜기
        if (_currentEffect == null)
        {
            Debug.Log("[FlamethrowerWeapon] flameEffectPrefab Instantiate");
            _currentEffect = Instantiate(flameEffectPrefab, firePoint.position, firePoint.rotation, firePoint);
        }
        else
        {
            Debug.Log("[FlamethrowerWeapon] flameEffect Already Exists");
        }
        _currentEffect.SetActive(true);
    }


    public override void StopAttack()
    {
        Debug.Log("[FlamethrowerWeapon] StopAttack 호출");
        _isFiring = false;
        if (_currentFlame != null)
            _currentFlame.SetActive(false);

        if (_currentEffect != null)
            _currentEffect.SetActive(false);
    }
}
