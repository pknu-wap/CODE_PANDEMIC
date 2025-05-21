using UnityEngine;
using UnityEngine.InputSystem;

public class FlamethrowerWeapon : WeaponBase
{
    [SerializeField] private GameObject flameEffect;
    [SerializeField] private GameObject hitboxObject;
    [SerializeField] private GameObject firePoint;

    private Animator _animator;
    private bool isFiring = false;
    private bool isHoldingFire = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        RotateToMouse();

        // 마우스 좌클릭 중일 때만 공격 지속
        if (isHoldingFire)
        {
            Debug.Log("isHoldingFire: true");
            if (!isFiring && CanFire())
            {
                Debug.Log("조건 만족: StartFiring 호출 예정");
                StartFiring();
            }

            // 탄약 다 떨어지면 정지 + 리로드
            if (_currentAmmo <= 0 && !_isReloading)
            {
                StopAttack();
                Reload();
            }
        }
        else if (isFiring)
        {
            StopAttack();
        }
    }

    public override void Attack(PlayerController owner)
    {
        if (!CanFire()) return;
        SetNextFireTime();
        _currentAmmo--;

        if (_currentAmmo <= 0)
        {
            Reload();
        }
    }

    public override void StopAttack()
    {
        isHoldingFire = false;
        isFiring = false;

        flameEffect?.SetActive(false);
        hitboxObject?.SetActive(false);

        if (_animator != null)
        {
            _animator.SetBool("Fire", false);
        }

        Debug.Log("화염방사기 발사 중지");
    }

    private void StartFiring()
    {
        Debug.Log("🔥 StartFiring() 실행됨");
        if (!CanFire()) return;

        isFiring = true;
        SetNextFireTime();
        _currentAmmo--;

        flameEffect?.SetActive(true);
        hitboxObject?.SetActive(true);

        // 데미지 전달
        FlameHitbox hitbox = hitboxObject.GetComponent<FlameHitbox>();
        if (hitbox != null)
        {
            hitbox.SetInfo(_weaponData);
        }

        if (_animator != null)
        {
            _animator.SetBool("Fire", true);
        }

        Debug.Log("화염방사기 발사 시작");
    }

    private void RotateToMouse()
    {
        if (transform == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        direction.z = 0f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}