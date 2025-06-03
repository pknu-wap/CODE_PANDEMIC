using UnityEngine;
using System.Collections;


public abstract class WeaponBase : MonoBehaviour
{
    protected WeaponData _weaponData;
    private float _nextFireTime;
    private bool isFacingRight = true;
    private Vector3 _originalScale;
    protected bool _isReloading = false;
    public int _currentBullet;
    protected bool _isAttacking = false;
    public bool IsReloading => _isReloading;

    public SpriteRenderer weaponSpriteRenderer;
    public WeaponData WeaponInfo
    {
        get { return _weaponData; }
        private set { _weaponData = value; }
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
            if (CanFire(owner))
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

    void Awake()
    {
        // prefab의 원래 localScale만 저장
        _originalScale = transform.localScale;
    }


    void LateUpdate()
    {
        // 부모 scale.x 영향 상쇄
        if (transform.parent != null)
        {
            Vector3 parentScale = transform.parent.lossyScale;
            Vector3 desiredScale = transform.localScale;

            // 부모가 x축 음수(flip)면 자식 x축도 반대로 뒤집어 상쇄
            float xSign = parentScale.x < 0 ? -1 : 1;

            transform.localScale = new Vector3(xSign * Mathf.Abs(desiredScale.x), desiredScale.y, desiredScale.z);
        }
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
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // y축 반전 (아래쪽 바라볼 때)
        if (angle > 90f || angle < -90f)
        {
            // 아래 방향이면 y축 -1 (스프라이트 반전)
            transform.localScale = new Vector3(_originalScale.x, -Mathf.Abs(_originalScale.y), _originalScale.z);
        }
        else
        {
            // 위쪽 방향이면 원래대로
            transform.localScale = new Vector3(_originalScale.x, Mathf.Abs(_originalScale.y), _originalScale.z);
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

        Managers.Event.InvokeEvent("Reload", _weaponData);
        yield return CoroutineHelper.WaitForSeconds(_weaponData.ReloadTime);
        _currentBullet = _weaponData.BulletCount;
        Managers.Event.InvokeEvent("BulletUpdated", _currentBullet);
        _isReloading = false;
        Debug.Log("Reload complete");
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

}