using UnityEngine;
using System.Collections;

public class HybridWeapon : WeaponBase
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackAngle = 120f;
    [SerializeField] private float FireRate = 0.2f;
    [SerializeField] private GameObject swingEffectPrefab;
    [SerializeField] private float _rotationSpeed = 720f;

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
                float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );

                // flipY
                SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                {
                    float currentZ = transform.eulerAngles.z;
                    sr.flipY = (currentZ > 90f && currentZ < 270f);
                }

                _currentAngle = transform.eulerAngles.z;
            }
        }
    }

    public override void Attack(PlayerController owner)
    {
        if (_isAttacking) return;
        _isAttacking = true;

        if (swingEffectPrefab != null)
        {
            float effectOffset = 1.0f;
            float effectAngle = _currentAngle + 20f;
            Vector3 effectDir = new Vector3(Mathf.Cos(effectAngle * Mathf.Deg2Rad), Mathf.Sin(effectAngle * Mathf.Deg2Rad), 0);
            Vector3 effectPos = attackPoint.position + effectDir * effectOffset;

            GameObject effect = Instantiate(swingEffectPrefab, effectPos, Quaternion.Euler(0, 0, _currentAngle));
            Destroy(effect, 0.3f);
        }

        StartCoroutine(SmoothSwingAttack());
    }
    private IEnumerator SmoothSwingAttack()
    {
        float swingDuration = 0.1f; // 휘두르기 시간: 빠르게
        float elapsed = 0f;

        float swingStart = _currentAngle;
        float swingEnd = _currentAngle + attackAngle;

        Quaternion startRot = Quaternion.Euler(0, 0, swingStart);
        Quaternion endRot = Quaternion.Euler(0, 0, swingEnd);

        while (elapsed < swingDuration)
        {
            float t = elapsed / swingDuration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;

        //  타격은 휘두른 후 즉시 (더 현실감 있게 만들고 싶으면 중간에 넣을 수도 있음)
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

        //  복귀는 굳이 코루틴으로 천천히 안 해도 괜찮음
        yield return new WaitForSeconds(0.05f); // 딜레이 약간
        ResetAttack(); // 내부에서 _currentAngle 기준으로 원래 회전값으로 복귀함
    }

    private void Throw()
    {
        _isAttacking = true;
        _isThrown = true;

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
        yield return new WaitForSeconds(FireRate);
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