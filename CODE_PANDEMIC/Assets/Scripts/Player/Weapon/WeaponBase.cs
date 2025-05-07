using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected WeaponData _weaponData;
    private float _nextFireTime;
    private bool isFacingRight = true;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;

    private Vector3 _originalScale;

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
        _nextFireTime = 0f;
        _weaponData = data;
    }

    public abstract void Attack();

    public virtual void Reload()
    {
        Debug.Log("Reloading...");
    }

    protected bool CanFire()
    {
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

        // flip은 스프라이트 반전용으로만 처리 (scale.y 반전)
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