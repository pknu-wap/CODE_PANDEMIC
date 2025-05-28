using UnityEngine;
using System.Collections;


public abstract class WeaponBase : MonoBehaviour
{
    protected WeaponData _weaponData;
    private float _nextFireTime;
    private bool isFacingRight = true;
    private Vector3 _originalScale;
    protected bool _isReloading = false;
    protected int _currentBullet;
    protected bool _isAttacking = false;
    public bool IsReloading => _isReloading;

    public SpriteRenderer weaponSpriteRenderer;
    public WeaponData Data
    {
        get { return _weaponData; }
        private set { _weaponData = value; }
    }
    public void SetWeaponFlip(bool isFacingLeft)
    {
        // x축만 플립: 왼쪽 바라볼 때 -1, 오른쪽 바라볼 때 1
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isFacingLeft ? -1 : 1);
        transform.localScale = scale;
    }
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

  
    void Update()
    {
        RotateToMouse();
    }

    private void RotateToMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.z = 0f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        bool shouldFaceRight = direction.x >= 0;

        if (shouldFaceRight != isFacingRight)
        {
            isFacingRight = shouldFaceRight;

            Vector3 localScale = transform.localScale;
            localScale.y *= -1;
            transform.localScale = localScale;
        }
    }

    public void SetInfo(WeaponData data)
    {
        _weaponData = data;
        _currentBullet = _weaponData.BulletCount;
        _isReloading = false;
        _nextFireTime = 0f;
    }

    public virtual void Reload()
    {
        if (_isReloading || _currentBullet == _weaponData.BulletCount) return;

        _isReloading = true;
        Debug.Log("Reloading...");
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        
        Managers.Event.InvokeEvent("Reload",_weaponData);
        yield return CoroutineHelper.WaitForSeconds(_weaponData.ReloadTime);
        _currentBullet = _weaponData.BulletCount;
        Managers.Event.InvokeEvent("BulletUpdated", _currentBullet);
        _isReloading = false;
        Debug.Log("Reload complete");
    }

    protected bool CanFire()
    {
        if (_isReloading) return false;

        if (_currentBullet <= 0)
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

  
}