using UnityEngine;
using System.Collections;

public class HybridWeapon : WeaponBase
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackAngle = 15f;
    [SerializeField] private float attackDuration = 0.1f;

    private bool _isAttacking = false;

    private enum HybridMode { Melee, Ranged }
    private HybridMode _currentMode = HybridMode.Melee;

    void Update()
    {
        // 모드 전환
        if (Input.GetKeyDown(KeyCode.T))
        {
            _currentMode = _currentMode == HybridMode.Melee ? HybridMode.Ranged : HybridMode.Melee;
            Debug.Log("[HybridWeapon] Mode Changed: " + _currentMode);
        }

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (_currentMode == HybridMode.Melee)
            {
                rb.isKinematic = true;      // 물리 해제(직접 위치 제어)
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                if (Input.GetMouseButtonDown(0) && !_isAttacking)
                {
                    PlayerController owner = GetComponentInParent<PlayerController>();
                    Attack(owner);
                }
            }
            else if (_currentMode == HybridMode.Ranged)
            {
                rb.isKinematic = false;     // 물리 적용(던지기/충돌)
                rb.gravityScale = 0f;
                if (Input.GetMouseButtonDown(0) && !_isAttacking)
                {
                    Throw();
                }
            }
        }
    }

    public override void Attack(PlayerController owner)
    {
        if (_isAttacking) return;
        _isAttacking = true;

        // 근접 공격
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDir = (mouseWorldPos - attackPoint.position).normalized;
        float angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;
        Quaternion attackRotation = Quaternion.Euler(0, 0, angle + attackAngle);

        StartCoroutine(RotateDuringAttack(attackRotation));

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (var hit in hits)
        {
            AI_Base enemy = hit.GetComponent<AI_Base>();
            if (enemy != null)
            {
                enemy.TakeDamage(_weaponData.Damage);
                Debug.Log("[HybridWeapon] Melee Hit: " + hit.name + " / Damage: " + _weaponData.Damage);
            }
        }

        Debug.Log("[HybridWeapon] Melee Attack. Enemies hit: " + hits.Length);
        Invoke(nameof(ResetAttack), attackDuration);
    }

    private void Throw()
    {
        _isAttacking = true;
        Debug.Log("[HybridWeapon] Throw (Ranged mode) 시작!");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 마우스 방향으로 투척
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 throwDir = ((Vector2)(mouseWorldPos - transform.position)).normalized;

            float throwPower = 10f; // 던지는 힘
            rb.isKinematic = false;
            rb.gravityScale = 0f;
            rb.velocity = throwDir * throwPower;
        }

        Invoke(nameof(ResetAttack), 0.2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_currentMode == HybridMode.Ranged)
        {
            AI_Base enemy = collision.gameObject.GetComponent<AI_Base>();
            if (enemy != null)
            {
                enemy.TakeDamage(_weaponData.Damage);
                Debug.Log("[HybridWeapon] Ranged Hit (Collision): " + collision.gameObject.name + " / Damage: " + _weaponData.Damage);
            }

            // 벽이든 적이든 부딪혔으면 무기 파괴
            int colLayer = collision.gameObject.layer;
            if (colLayer == LayerMask.NameToLayer("Wall") ||
                colLayer == LayerMask.NameToLayer("AttackObj") ||
                colLayer == LayerMask.NameToLayer("Interact") ||
                colLayer == LayerMask.NameToLayer("Default"))
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator RotateDuringAttack(Quaternion attackRotation)
    {
        Quaternion originalRotation = transform.rotation;
        transform.rotation = attackRotation;
        yield return new WaitForSeconds(attackDuration);
        transform.rotation = originalRotation;
    }


    private void DetachFromPlayer()
    {
        transform.SetParent(null, true); // true: 월드 좌표 보존

        // Rigidbody2D 물리적 설정
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.gravityScale = 0f;
        }

        var player = GetComponentInParent<PlayerController>();
        if (player != null)
        {
            var equip = player.GetComponent<EquipWeapon>();
            if (equip != null)
            {
                equip.UnEquipWeapon(); // 내부에서 _weapon = null, DestroyPrevWeapon() 등 호출
            }

            Debug.Log("부모 분리 전: " + transform.parent);
            transform.SetParent(null, true);
            Debug.Log("부모 분리 후: " + transform.parent);

        }
        transform.SetParent(null, true);

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        _isAttacking = false;
    }


    private void ResetAttack()
    {
        _isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
