using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{

    protected WeaponData _weaponData;
    private float _nextFireTime;
    private bool isFacingRight = true;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;

    private Vector3 _originalScale;
    public int ID
    {
        get { return _weaponData.TemplateID; }
     
    }

    void Awake()
    {
        // parent 영향 제거된 순수 스케일 저장
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

    public abstract void Attack(PlayerController owner);

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
    }

    private void RotateToMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.z = 0f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
        parentScale.x = Mathf.Approximately(parentScale.x, 0f) ? 0.0001f : parentScale.x;
        parentScale.y = Mathf.Approximately(parentScale.y, 0f) ? 0.0001f : parentScale.y;
        parentScale.z = Mathf.Approximately(parentScale.z, 0f) ? 0.0001f : parentScale.z;

        transform.localScale = new Vector3(
            _originalScale.x / parentScale.x,
            _originalScale.y / parentScale.y,
            _originalScale.z / parentScale.z
        );
    }
}