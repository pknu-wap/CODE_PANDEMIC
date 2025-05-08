using UnityEngine;

public class HybridWeaponBase : WeaponBase
{
    [SerializeField] private GameObject firePoint;
    [SerializeField] private float meleeRange = 1.5f;//�Ÿ� ����
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private bool shouldRotateWhileFlying = true; // ȸ�� ���� ����

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
                // ȸ�� �ӵ� ���� (������ ���� �ʿ���)
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
            Debug.Log("���� ���� �����.");
        }
        else
        {
            isThrown = true;
            direction = firePoint.transform.right.normalized;
            speed = _weaponData.BulletSpeed;
            damage = _weaponData.Damage;

            Debug.Log("��ôü�� ���������ϴ�.");
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
            Debug.Log("��ôü�� ���� �浹�߽��ϴ�.");
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
