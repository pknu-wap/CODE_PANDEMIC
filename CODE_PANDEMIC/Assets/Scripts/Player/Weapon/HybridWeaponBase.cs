using UnityEngine;

public class HybridWeaponBase : WeaponBase
{
    [SerializeField] private GameObject firePoint;
    [SerializeField] private float meleeRange = 1.5f;//거리 범위
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private bool shouldRotateWhileFlying = true; // 회전 여부 설정

    private bool isThrown = false;
    private Vector3 direction;
    private float speed;
    private int damage;

    private void Update()
    {
        if (isThrown)
        {
            transform.position += direction * speed * Time.deltaTime;

            if (shouldRotateWhileFlying)
            {
                // 회전 속도 조정 (각도는 조절 필요함)
                transform.Rotate(0, 0, 720 * Time.deltaTime);
            }
        }
    }

    public override void Attack(PlayerController owner)
    {
        if (!CanFire() || isThrown) return;
        SetNextFireTime();

        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, meleeRange, enemyLayer);

        if (nearbyEnemies.Length > 0)
        {
            foreach (var enemyCollider in nearbyEnemies)
            {
                ApplyDamageWithKnockback(enemyCollider, _weaponData.Damage);
            }
            Debug.Log("근접 공격 실행됨.");
        }
        else
        {
            isThrown = true;
            direction = firePoint.transform.right.normalized;
            speed = _weaponData.BulletSpeed;
            damage = _weaponData.Damage;

            Debug.Log("투척체가 던져졌습니다.");
            Destroy(gameObject, 3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isThrown) return;

        AI_Base enemy = other.GetComponent<AI_Base>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("투척체가 적과 충돌했습니다.");
            Destroy(gameObject);
        }
    }

    private void ApplyDamageWithKnockback(Collider2D target, int damage)
    {
        AI_Base enemy = target.GetComponent<AI_Base>();
        if (enemy != null)
        {
            Vector3 knockbackDir = (enemy.transform.position - transform.position).normalized;
            enemy.TakeDamage(damage, knockbackDir);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
