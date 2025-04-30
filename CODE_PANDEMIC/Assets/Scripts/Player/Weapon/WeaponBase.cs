using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
  
    protected WeaponData _weaponData;
    private float _nextFireTime;
    private bool isFacingRight = true;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;

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
        Debug.Log( " Reloading...");
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
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        transform.right = direction;

        Vector3 parentScale = transform.root.lossyScale;
        Vector3 inverseScale = new Vector3(1f / parentScale.x, 1f / parentScale.y, 1f / parentScale.z);
        transform.localScale = inverseScale;
    }
}
