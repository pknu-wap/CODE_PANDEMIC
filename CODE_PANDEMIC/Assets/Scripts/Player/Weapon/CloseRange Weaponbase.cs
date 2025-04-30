using UnityEngine;

public class CloseRangeWeaponBase : WeaponBase
{
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected Transform attackPoint;

    public override void Attack()
    {
        if (!CanFire()) return;

        Debug.Log( " performed a short-range attack!");
        SetNextFireTime();
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, targetLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
        }
    }

    public override void Reload()
    {

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
