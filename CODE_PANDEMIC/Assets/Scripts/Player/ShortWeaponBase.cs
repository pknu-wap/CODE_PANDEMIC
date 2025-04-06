using UnityEngine;

public class ShortWeaponBase : WeaponBase
{
    public override void Attack()
    {
        if (!CanFire()) return;

        Debug.Log(weaponName + " performed a short-range attack!");
        SetNextFireTime();

    }
}
