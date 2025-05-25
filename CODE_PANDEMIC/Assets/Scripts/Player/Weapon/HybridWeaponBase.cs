using UnityEngine;

public class HybridWeaponBaseWeaponBase : WeaponBase
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 1.2f;
    [SerializeField] private LayerMask enemyLayer;

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

        // 공격 판정
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            enemyLayer
        );
        foreach (var hit in hits)
        {
            AI_Base enemy = hit.GetComponent<AI_Base>();
            Vector2 toTarget = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;
            float angle = Vector2.Angle(attackDir, toTarget);

            if (angle <= 15f) // 30도 부채꼴 (±15도)
            {
                AI_Base ai = hit.GetComponent<AI_Base>();
                if (ai != null)
                {
                    ai.TakeDamage(_weaponData.Damage);
                    hitAny = true;
                }
            }
        }

        if (hitAny)
        {
            Debug.Log("부채꼴 근접 공격 성공");
        }
        else
        {
            StartThrow();
        }
    }


    private void Update()
    {
        if (_isThrown)
        {
            transform.position += _direction * throwSpeed * Time.deltaTime;

            if (shouldRotateWhileFlying)
                transform.Rotate(0, 0, 720 * Time.deltaTime);

            if (Vector3.Distance(_startPos, transform.position) >= _weaponData.Range)
            {
                _isThrown = false;
                _isReturning = true;
            }
        }
        else if (_isReturning)
        {
            Vector3 backDir = (_player.position - transform.position).normalized;
            transform.position += backDir * throwSpeed * returnSpeedMultiplier * Time.deltaTime;

            if (shouldRotateWhileFlying)
                transform.Rotate(0, 0, 720 * Time.deltaTime);

            if (Vector3.Distance(transform.position, _player.position) < 0.3f)
            {
                _isReturning = false;
                _isFlying = false;
                AttachToPlayer();
            }
        }
    }

    private void AttachToPlayer()
    {
        //transform.parent = _player;
        //transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.identity;
        //Debug.Log("무기 복귀 완료");
        
        //if (!CanFire() || isThrown) return;
        //SetNextFireTime();
        //_currentAmmo--;
    
        //Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, meleeRange, enemyLayer);

        //if (nearbyEnemies.Length > 0)
        //{
        //    foreach (var enemyCollider in nearbyEnemies)
        //    {
        //        ApplyDamageWithKnockback(enemyCollider, _weaponData.Damage);
        //    }
        //    Debug.Log("근접 공격 실행됨.");
        //}
        //else
        //{
        //    isThrown = true;
        //    direction = firePoint.transform.right.normalized;
        //    speed = _weaponData.BulletSpeed;
        //    damage = _weaponData.Damage;

        //    Debug.Log("투척체가 던져졌습니다.");
        //    Destroy(gameObject, 3f);
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isFlying)
        {
            AI_Base enemy = other.GetComponent<AI_Base>();
            if (enemy != null)
            {
                enemy.TakeDamage(_weaponData.Damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
