using UnityEngine;
using System.Collections;

public abstract class WeaponBase : MonoBehaviour
{
    protected WeaponData _weaponData;
    protected bool _isReloading = false;
    protected bool _isAttacking = false;

    private float _nextFireTime;
    private Vector3 _originalScale;

    public int _currentBullet;
    public bool IsReloading => _isReloading;

    public SpriteRenderer weaponSpriteRenderer;

    public WeaponData WeaponInfo
    {
        get => _weaponData;
        private set => _weaponData = value;
    }

    public int ID => _weaponData.TemplateID;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        RotateToMouse();
    }

    private void LateUpdate()
    {
        CancelParentScaleEffect();
    }

    public void SetInfo(WeaponData data)
    {
        _weaponData = data;
        _currentBullet = _weaponData.BulletCount;
        _isReloading = false;
        _nextFireTime = 0f;
    }

    public virtual void StartAttack(PlayerController owner)
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            StartCoroutine(ContinuousFire(owner));
        }
    }

    public virtual void StopAttack()
    {
        _isAttacking = false;
    }

    private IEnumerator ContinuousFire(PlayerController owner)
    {
        while (_isAttacking)
        {
            if (CanFire(owner))
            {
                Attack(owner);
            }

            yield return null;
        }
    }

    protected bool CanFire(PlayerController owner)
    {
        if (_isReloading) return false;

        if (_currentBullet <= 0)
        {
            Reload();
            return false;
        }

        return Time.time >= owner.GlobalNextFireTime;
    }

    protected void SetNextFireTime(PlayerController owner)
    {
        owner.GlobalNextFireTime = Time.time + _weaponData.FireRate;
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
        Managers.Event.InvokeEvent("Reload", _weaponData);
        yield return CoroutineHelper.WaitForSeconds(_weaponData.ReloadTime);

        _currentBullet = _weaponData.BulletCount;
        Managers.Event.InvokeEvent("BulletUpdated", _currentBullet);
        _isReloading = false;
        Debug.Log("Reload complete");
    }

    private void RotateToMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.z = 0f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        bool isFlipped = angle > 90f || angle < -90f;
        float yScale = isFlipped ? -Mathf.Abs(_originalScale.y) : Mathf.Abs(_originalScale.y);

        transform.localScale = new Vector3(_originalScale.x, yScale, _originalScale.z);
    }

    private void CancelParentScaleEffect()
    {
        if (transform.parent == null) return;

        Vector3 parentScale = transform.parent.lossyScale;
        float xSign = parentScale.x < 0 ? -1 : 1;

        transform.localScale = new Vector3(
            xSign * Mathf.Abs(transform.localScale.x),
            transform.localScale.y,
            transform.localScale.z
        );
    }

    public abstract void Attack(PlayerController owner);
}