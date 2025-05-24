using UnityEngine;
using System.Collections;


public abstract class WeaponBase : MonoBehaviour
{
    protected WeaponData _weaponData;
    private float _nextFireTime;
    private bool isFacingRight = true;
    private Vector3 _originalScale;
    protected bool _isReloading = false;
    protected int _currentAmmo;
    protected bool _isAttacking = false;
    public bool IsReloading => _isReloading;

    public SpriteRenderer weaponSpriteRenderer;

    public virtual void StopAttack()
    {
        _isAttacking = false;
    }

    public virtual void StartAttack(PlayerController owner)
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            StartCoroutine(ContinuousFire(owner));
        }
    }

    private IEnumerator ContinuousFire(PlayerController owner)
    {
        while (_isAttacking)
        {
            if (CanFire())
            {
                Attack(owner);
            }

            yield return null;
        }
    }

    public abstract void Attack(PlayerController owner);

    public int ID
    {
        get { return _weaponData.TemplateID; }
     
    }

    //void Awake()
    //{
    //    Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
    //    _originalScale = new Vector3(
    //        transform.localScale.x * parentScale.x,
    //        transform.localScale.y * parentScale.y,
    //        transform.localScale.z * parentScale.z
    //    );
    //}

    void Update()
    {
        RotateToMouse();
    }

    private void RotateToMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mouseWorldPosition - transform.position;
        dir.z = 0f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 무기(Weapon) 오브젝트는 무조건 마우스를 향해서만 회전한다.
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // ***localScale flip 완전 제거***
        // transform.localScale = new Vector3(mouseWorldPosition.x < transform.position.x ? -1 : 1, 1, 1); // <<<< 이 코드 삭제!

        // flip 함수도 제거(더 이상 사용 X)
        // SetWeaponFlip, SetFacingDirection 등 다 삭제
    }


    public void SetInfo(WeaponData data)
    {
        _weaponData = data;
        _currentAmmo = _weaponData.BulletCount;
        _isReloading = false;
        _nextFireTime = 0f;
    }

    public virtual void Reload()
    {
        if (_isReloading || _currentAmmo == _weaponData.BulletCount) return;

        _isReloading = true;
        Debug.Log("Reloading...");
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        
        Managers.Event.InvokeEvent("Reload",_weaponData);
        yield return CoroutineHelper.WaitForSeconds(_weaponData.ReloadTime);
        _currentAmmo = _weaponData.BulletCount;
        Managers.Event.InvokeEvent("BulletUpdated", _currentAmmo);
        _isReloading = false;
        Debug.Log("Reload complete");
    }

    protected bool CanFire()
    {
        if (_isReloading) return false;

        if (_currentAmmo <= 0)
        {
            Reload();
            return false;
        }

        return Time.time >= _nextFireTime;
    }

    protected void SetNextFireTime()
    {
        _nextFireTime = Time.time + _weaponData.FireRate;
    }

    //public void SetFacingDirection(bool facingRight)
    //{
    //    isFacingRight = facingRight;

    //    float direction = facingRight ? 1f : -1f;

    //    Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
    //    transform.localScale = new Vector3(
    //        (_originalScale.x * direction) / parentScale.x,
    //        _originalScale.y / parentScale.y,
    //        _originalScale.z / parentScale.z
    //    );
    //}

    public void SetWeaponFlip(bool isFacingLeft)
    {
        transform.localScale = new Vector3(isFacingLeft ? -1 : 1, 1, 1);
    }
}