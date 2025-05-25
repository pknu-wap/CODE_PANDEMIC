using UnityEngine;

public class HybridWeaponBase : WeaponBase
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 1.2f; // 공격 범위
    [SerializeField] private LayerMask enemyLayer;     // 적 Layer

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Attack(PlayerController owner)
    {
        if (!CanFire()) return;
        SetNextFireTime();

        if (_animator != null)
            _animator.SetTrigger("Attack");

        if (attackPoint != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - attackPoint.transform.position;
            direction.z = 0f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            attackPoint.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (weaponSpriteRenderer != null)
            {
                // flipY: 마우스가 attackPoint(총구)보다 왼쪽에 있으면 true, 아니면 false
                weaponSpriteRenderer.flipY = (mousePos.x < attackPoint.transform.position.x);
                // flipX는 사용하지 마
            }
        }

        // 공격 판정
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            enemyLayer
        );
        foreach (var hit in hits)
        {
            AI_Base enemy = hit.GetComponent<AI_Base>();
            if (enemy != null)
            {
                enemy.TakeDamage(_weaponData.Damage);
                // 이펙트, 넉백 등 추가 가능
            }
        }
    }

    // 에디터에서 공격 범위 확인용 Gizmo
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
