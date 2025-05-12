using UnityEngine;

public class FlamethrowerWeapon : WeaponBase
{
    [SerializeField] private GameObject flameEffect;
    [SerializeField] private GameObject hitboxObject;

    private bool isFiring = false;

    private void Update()
    {
        if (isFiring)
        {
            SetNextFireTime();
        }
    }

    public override void Attack(PlayerController owner)
    {
        if (!CanFire()) return;
        isFiring = true;

        flameEffect.SetActive(true);
        hitboxObject.SetActive(true);
    }


    public override void StopAttack()
    {
        isFiring = false;

        flameEffect.SetActive(false);
        hitboxObject.SetActive(false);
    }
}
