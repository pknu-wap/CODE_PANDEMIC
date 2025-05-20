using UnityEngine;

public class HybridWeaponBase : WeaponBase
{
    [SerializeField] private GameObject firePoint;
    [SerializeField] private float meleeRange = 1.5f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Throw Settings")]
    [SerializeField] private float throwSpeed = 8f;
    [SerializeField] private bool shouldRotateWhileFlying = true;

    private bool isThrown = false;
    private bool isReturning = false;

    private Vector3 direction;
    private Vector3 startPosition;
    private float speed;
    private int damage;

    private void Update()
    {
        if (isThrown)
        {
            transform.position += direction * speed * Time.deltaTime;

            if (shouldRotateWhileFlying)
                transform.Rotate(0, 0, 720 * Time.deltaTime);

            float distanceFromStart = Vector3.Distance(transform.position, startPosition);

            // 최대 거리 도달 → 되돌아오기
            if (!isReturning && distanceFromStart >= _weaponData.Range)
            {
                isReturning = true;
                direction = (startPosition - transform.position).normalized;
            }
        }
    }

    public override void Attack(PlayerController owner)
    {
        if (!CanFire() || isThrown) return;

        SetNextFireTime();
        _currentAmmo--;

        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, meleeRange, enemyLayer);
        if (nearbyEnemies.Length > 0)
        {
            foreach (var enemyCollider in nearbyEnemies)
                ApplyDamageWithKnockback(enemyCollider, _weaponData.Damage);

            Debug.Log("근접 공격 실행됨.");
        }
        else
        {
            isThrown = true;
            isReturning = false;
            startPosition = transform.position;

            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            direction = (mouseWorld - transform.position).normalized;

            speed = throwSpeed;
            damage = _weaponData.Damage;

            Debug.Log($"투척 시작: 방향 {direction}, 속도 {speed}");

            transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isThrown) return;

        if (isReturning && other.CompareTag("Player"))
        {
            Debug.Log("부메랑이 플레이어에게 돌아옴");
            Destroy(gameObject); // 또는 ReturnToPool()
            return;
        }

        AI_Base enemy = other.GetComponent<AI_Base>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("적과 충돌 후 되돌아감");
        }

        if (!isReturning)
        {
            isReturning = true;
            direction = (startPosition - transform.position).normalized;
            Debug.Log("벽에 부딪혀서 되돌아감");
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
