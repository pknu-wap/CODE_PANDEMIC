using UnityEngine;

public class FlamethrowerWeapon : WeaponBase
{
    [SerializeField] private GameObject flameHitboxPrefab;
    [SerializeField] private GameObject flameEffectPrefab;
    [SerializeField] private Transform firePoint;

    private GameObject _currentFlame;
    private GameObject _currentEffect;
    private bool _isFiring = false;

    public override void Attack(PlayerController owner)
    {
        Debug.Log("[FlamethrowerWeapon] Attack 호출");
        StartAttack(owner);
        UpdateFirePointRotation();

    }
    private void UpdateFirePointRotation()
    {
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
        if (_isFiring || _isReloading) return;
        _isFiring = true;

        if (_currentFlame == null)
        {
            CreateFlameAndEffect();
        }
        else
        {
            UpdateFlameDamage();
            if (_currentEffect != null)
                _currentEffect.SetActive(true);
        }

        _currentFlame.SetActive(true);
        if (_currentEffect != null)
            _currentEffect.SetActive(true);
    }

    private void CreateFlameAndEffect()
    {
        _currentFlame = Instantiate(flameHitboxPrefab, firePoint.position, firePoint.rotation, firePoint);
        FlameHitbox hitbox = _currentFlame.GetComponent<FlameHitbox>();
        hitbox.SetInfo(_weaponData);

        if (flameEffectPrefab != null)
        {
            _currentEffect = Instantiate(flameEffectPrefab, _currentFlame.transform);
            _currentEffect.transform.localPosition = Vector3.zero;
        }
    }

    private void UpdateFlameDamage()
    {
        FlameHitbox hitbox = _currentFlame.GetComponent<FlameHitbox>();
        hitbox.SetInfo(_weaponData);
    }

    public override void StopAttack()
    {
        _isFiring = false;

        if (_currentFlame != null)
            _currentFlame.SetActive(false);

        if (_currentEffect != null)
            _currentEffect.SetActive(false);
    }
}
