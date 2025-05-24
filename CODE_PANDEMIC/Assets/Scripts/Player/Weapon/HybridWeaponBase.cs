using UnityEngine;

public class HybridBoomerangWeapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Settings")]
    [SerializeField] private float meleeCheckRadius = 5f;
    [SerializeField] private float throwSpeed = 10f;
    [SerializeField] private float returnSpeedMultiplier = 1.2f;
    [SerializeField] private bool shouldRotateWhileFlying = true;

    private WeaponData _weaponData;
    private Transform _player;
    private Vector3 _startPos;
    private Vector3 _direction;
    private bool _isThrown = false;
    private bool _isReturning = false;
    private bool _isFlying = false;

    public void SetInfo(WeaponData data, Transform player)
    {
        _weaponData = data;
        _player = player;
        AttachToPlayer(); // 처음엔 항상 붙어 있어야 함
    }

    private void StartThrow()
    {
        _isFlying = true;
        _isThrown = true;
        _startPos = transform.position;

        // 방향 계산
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _direction = (mousePos - firePoint.position).normalized;
        _direction.z = 0;

        transform.parent = null; // 플레이어로부터 분리
    }

    public void Attack()
    {
        if (_isFlying) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, meleeCheckRadius, enemyLayer);
        Vector2 attackDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        bool hitAny = false;
        foreach (var hit in hits)
        {
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
                Debug.Log("날아가는 중 적 타격");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, meleeCheckRadius);
    }
}
