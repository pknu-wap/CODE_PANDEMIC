using UnityEngine;

public class HybridWeaponBase : WeaponBase
{
    [Header("Hybrid Weapon Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float meleeCheckRadius = 3f;
    [SerializeField] private float returnSpeed = 15f;

    private bool isRangedMode = false;
    private bool _isThrown = false;
    private bool _isReturning = false;
    private Vector3 _startPosition;
    private Vector3 _throwDirection;
    private float _throwDistance;
    private Transform _playerTransform;
    private Transform _weaponSocket;
    private Rigidbody2D _rb;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            isRangedMode = !isRangedMode;
            Debug.Log(isRangedMode ? "[모드] 원거리" : "[모드] 근거리");
        }
        if (_isThrown)
        {
            Debug.Log($"[Update] Thrown: {_isThrown}, Returning: {_isReturning}, Pos: {transform.position}");
            if (!_isReturning)
            {
                float traveled = Vector3.Distance(_startPosition, transform.position);
                Debug.Log($"[Update] Traveled: {traveled} / ThrowDistance: {_throwDistance}");
                if (traveled >= _throwDistance)
                {
                    Debug.Log("[Update] 리턴 상태 진입");
                    _isReturning = true;
                    if (_rb != null) _rb.velocity = Vector2.zero;
                    if (_rb != null) _rb.isKinematic = true;
                }
            }
            else
            {
                if (_rb != null) Destroy(_rb);
                Vector3 toPlayer = (_weaponSocket.position - transform.position).normalized;
                transform.position += toPlayer * returnSpeed * Time.deltaTime;
                Debug.Log($"[Update] 리턴중. PlayerDist: {Vector3.Distance(transform.position, _weaponSocket.position)}");
                if (Vector3.Distance(transform.position, _weaponSocket.position) < 0.3f)
                {
                    Debug.Log("[Update] 플레이어 도착, 무기 재장착");
                    _isThrown = false;
                    _isReturning = false;
                    transform.SetParent(_weaponSocket);
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.identity;
                    Collider2D col = GetComponent<Collider2D>();
                    if (col != null) col.enabled = true;
                }
            }
        }
    }

    public override void Attack(PlayerController owner)
    {
        Debug.Log("[Attack] 호출됨: isRangedMode=" + isRangedMode);
        if (_weaponSocket == null)
            _weaponSocket = owner.transform.Find("WeaponSocket") ?? owner.transform;

        if (!isRangedMode)
        {
            // ===== 근접 공격 =====
            Collider2D[] hits = Physics2D.OverlapCircleAll(owner.transform.position, meleeCheckRadius, enemyLayer);
            Debug.Log("[Attack] 근접 타겟 탐색: " + hits.Length);

            if (hits.Length > 0)
            {
                float minDist = float.MaxValue;
                Transform nearestEnemy = null;
                Collider2D nearestCol = null;
                foreach (var hit in hits)
                {
                    float dist = Vector3.Distance(owner.transform.position, hit.transform.position);
                    Debug.Log($"[Attack] 근접 후보: {hit.name}, Dist: {dist}");
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearestEnemy = hit.transform;
                        nearestCol = hit;
                    }
                }

                if (nearestEnemy != null && minDist <= meleeCheckRadius)
                {
                    Debug.Log("[Attack] 근접 공격!");
                    AI_Base enemy = nearestCol.GetComponent<AI_Base>();
                    if (enemy != null)
                    {
                        Debug.Log($"[Attack] 근접 적 타격: {enemy.name}, Damage: {_weaponData.Damage}");
                        enemy.TakeDamage(_weaponData.Damage);
                    }
                    return; // 근접 공격시 종료
                }
            }
            Debug.Log("[Attack] 근접 대상 없음. 아무 일도 안 함.");
        }
        else
        {
            // ===== 원거리 투척 =====
            Debug.Log("[Attack] 투척 공격 실행!");
            ThrowWeapon(owner);
        }
    }



    private void ThrowWeapon(PlayerController owner)
    {
        Debug.Log("[ThrowWeapon] 호출");
        _playerTransform = owner.transform;
        if (_weaponSocket == null)
            _weaponSocket = _playerTransform.Find("WeaponSocket") ?? _playerTransform;

        _isThrown = true;
        _isReturning = false;
        _startPosition = transform.position;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        _throwDirection = (mouseWorld - transform.position).normalized;
        _throwDistance = Mathf.Min(_weaponData.Range, Vector3.Distance(transform.position, mouseWorld));
        transform.SetParent(null);

        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) _rb = gameObject.AddComponent<Rigidbody2D>();
        _rb.isKinematic = false;
        _rb.gravityScale = 0;
        _rb.velocity = _throwDirection * _weaponData.Range;
        _rb.angularVelocity = 720f;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        Debug.Log($"[ThrowWeapon] 던짐! Dir: {_throwDirection}, Dist: {_throwDistance}, Velocity: {_rb.velocity}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[OnTriggerEnter2D] " + other.gameObject.name + $" Thrown:{_isThrown}, Returning:{_isReturning}");
        if (!_isThrown || _isReturning) return;

        AI_Base enemy = other.GetComponent<AI_Base>();
        if (enemy != null)
        {
            Debug.Log($"[OnTriggerEnter2D] 투척 적 피격! 대상:{enemy.name}, Damage:{_weaponData.Damage}");
            enemy.TakeDamage(_weaponData.Damage);
            _isReturning = true;
            if (_rb != null) _rb.velocity = Vector2.zero;
            if (_rb != null) _rb.isKinematic = true;
        }
    }
}