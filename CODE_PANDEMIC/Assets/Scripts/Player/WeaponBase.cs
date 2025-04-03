using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName;
    public float damage;
    public float fireRate;
    public float range;
    private float nextFireTime;

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
}
