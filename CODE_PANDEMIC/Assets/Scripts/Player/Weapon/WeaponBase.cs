using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName;
    public float damage;
    public float fireRate;
    public float range;
    private float nextFireTime;

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

    void RotateToMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
