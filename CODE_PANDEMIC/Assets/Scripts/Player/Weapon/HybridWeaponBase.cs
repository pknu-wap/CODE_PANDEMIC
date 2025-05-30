using UnityEngine;
using System.Collections;

public class HybridWeapon : WeaponBase
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackAngle = 90f;
     private float attackDuration = 0.3f;
    [SerializeField] private GameObject swingEffectPrefab;

    private bool _isThrown = false;
    private bool _isAttacking = false;
    private float _currentAngle = 0f;

    private enum HybridMode { Melee, Ranged }
    private HybridMode _currentMode = HybridMode.Melee;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _currentMode = _currentMode == HybridMode.Melee ? HybridMode.Ranged : HybridMode.Melee;
            Debug.Log("[HybridWeapon] Mode Changed: " + _currentMode);
        }

        if (_isThrown) return;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            if (_currentMode == HybridMode.Melee)
            {
                if (Input.GetMouseButtonDown(0) && !_isAttacking)
                {
                    PlayerController owner = GetComponentInParent<PlayerController>();
                    Attack(owner);
                }
            }
            else if (_currentMode == HybridMode.Ranged)
            {
                if (Input.GetMouseButtonDown(0) && !_isAttacking)
                {
                    Throw();
                }
            }

            if (!_isAttacking && transform.parent != null)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = (mouseWorldPos - attackPoint.position).normalized;
                _currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, _currentAngle);

                // flipY
                SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                    sr.flipY = (_currentAngle > 90f || _currentAngle < -90f);
            }
        }
    }


    public override void Attack(PlayerController owner)
    {
        if (_isAttacking) return;
        _isAttacking = true;


        float swingAngle = _currentAngle + attackAngle;
        transform.rotation = Quaternion.Euler(0, 0, swingAngle);

        //Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 attackDir = (mouseWorldPos - attackPoint.position).normalized;
        //float angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;
        //float swingAngle = angle + attackAngle;


        // 이펙트 생성
        if (swingEffectPrefab != null)
        {
            GameObject effect = Instantiate(swingEffectPrefab, attackPoint.position, Quaternion.Euler(0, 0, swingAngle));
            Destroy(effect, 0.3f);  // 이펙트의 실제 애니메이션 길이에 맞춰 0.2f를 조절하세요
        }

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
            sr.flipY = (swingAngle > 90f || swingAngle < -90f);

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
        _isThrown = true; // 던진 상태로 표시

        Debug.Log("[HybridWeapon] Throw (Ranged mode) 시작!");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.gravityScale = 0f;

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 throwDir = ((Vector2)(mouseWorldPos - transform.position)).normalized;
            float throwPower = 10f;
            rb.velocity = throwDir * throwPower;
        }

        Invoke(nameof(ResetAttack), 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (_currentMode == HybridMode.Ranged)
        {
            AI_Base enemy = trigger.gameObject.GetComponent<AI_Base>();
            if (enemy != null)
            {
                enemy.TakeDamage(_weaponData.Damage);
                Debug.Log("[HybridWeapon] Ranged Hit (Trigger): " + trigger.gameObject.name + " / Damage: " + _weaponData.Damage);
            }

            int colLayer = trigger.gameObject.layer;
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
    private void ResetAttack()
    {
        _isAttacking = false;
        transform.rotation = Quaternion.Euler(0, 0, _currentAngle);

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
            sr.flipY = (_currentAngle > 90f || _currentAngle < -90f);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
