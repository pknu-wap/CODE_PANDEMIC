using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName;
    public float damage;
    public float fireRate;
    public float range;
    private float nextFireTime;

    private bool isFacingRight = true;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;

    void Update()
    {
        if (transform.root.CompareTag("Player"))
        {
            RotateToMouse();
        }
    }

    public abstract void Attack();

    public virtual void Reload()
    {
        Debug.Log(weaponName + " is reloading...");
    }

    protected bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

    protected void SetNextFireTime()
    {
        nextFireTime = Time.time + fireRate;
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
