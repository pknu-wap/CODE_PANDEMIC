using UnityEngine;
using System.Collections;


public abstract class WeaponBase : MonoBehaviour
{
    protected WeaponData _weaponData;
    private float _nextFireTime;
    private bool isFacingRight = true;
    private Vector3 _originalScale;
    public virtual void StopAttack() { }

    protected bool _isReloading = false;
    protected int _currentAmmo;

    public bool IsReloading => _isReloading;

    [SerializeField] private SpriteRenderer weaponSpriteRenderer;


    public int ID
    {
        get { return _weaponData.TemplateID; }
     
    }

    void Awake()
    {
        Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
        _originalScale = new Vector3(
            transform.localScale.x * parentScale.x,
            transform.localScale.y * parentScale.y,
            transform.localScale.z * parentScale.z
        );
    }

    void Update()
    {
        RotateToMouse();
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
        yield return new WaitForSeconds(_weaponData.ReloadTime);
        _currentAmmo = _weaponData.BulletCount;
        _isReloading = false;
        Debug.Log("Reload complete");
    }


    public abstract void Attack(PlayerController owner);

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

    public void SetFacingDirection(bool facingRight)
    {
        isFacingRight = facingRight;

        float direction = facingRight ? 1f : -1f;

        Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
        transform.localScale = new Vector3(
            (_originalScale.x * direction) / parentScale.x,
            _originalScale.y / parentScale.y,
            _originalScale.z / parentScale.z
        );
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


}