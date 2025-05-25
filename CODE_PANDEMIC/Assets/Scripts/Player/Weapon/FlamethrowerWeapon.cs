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
                // flipY: 마우스가 firePoint(총구)보다 왼쪽에 있으면 true, 아니면 false
                weaponSpriteRenderer.flipY = (mousePos.x < firePoint.transform.position.x);
                // flipX는 사용하지 마
            }
        }
    }


    public override void StartAttack(PlayerController owner)
    {
        if (_isFiring || _isReloading) return;
        _isFiring = true;

        // 불꽃 판정 오브젝트(히트박스) 켜기
        if (_currentFlame == null)
        {
            _currentFlame = Instantiate(flameHitboxPrefab, firePoint.position, firePoint.rotation, firePoint);
            FlameHitbox hitbox = _currentFlame.GetComponent<FlameHitbox>();
            hitbox.SetInfo(_weaponData);
        }
        _currentFlame.SetActive(true);

        // **불꽃 이펙트 켜기**
        if (_currentEffect == null)
        {
            _currentEffect = Instantiate(flameEffectPrefab, firePoint.position, firePoint.rotation, firePoint);
        }
        _currentEffect.SetActive(true);
    }

    public override void StopAttack()
    {
        _isFiring = false;
        if (_currentFlame != null)
            _currentFlame.SetActive(false);

        // **불꽃 이펙트 끄기**
        if (_currentEffect != null)
            _currentEffect.SetActive(false);
    }


}