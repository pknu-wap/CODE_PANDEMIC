using UnityEngine;

public class HybridWeaponBase : WeaponBase
{
    [Header("Hybrid Weapon Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float meleeCheckRadius = 3f; // 근접공격 범위
    [SerializeField] private float returnSpeed = 15f;      // 돌아올 때 속도

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
        if (_isThrown)
        {
            if (!_isReturning)
            {
                float traveled = Vector3.Distance(_startPosition, transform.position);
                if (traveled >= _throwDistance)
                {
                    _isReturning = true;
                    if (_rb != null) _rb.velocity = Vector2.zero;
                }
            }
            else
            {
                if (_rb != null) Destroy(_rb);

                Vector3 toPlayer = (_weaponSocket.position - transform.position).normalized;
                transform.position += toPlayer * returnSpeed * Time.deltaTime;

                if (Vector3.Distance(transform.position, _weaponSocket.position) < 0.3f)
                {
                    _isThrown = false;
                    _isReturning = false;
                    transform.SetParent(_weaponSocket);
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.identity;
                }
            }
        }
    }

    public override void Attack(PlayerController owner)
    {
        Debug.Log("[HybridWeaponBase] Attack 호출됨");
        if (_weaponSocket == null)
            _weaponSocket = owner.transform.Find("WeaponSocket") ?? owner.transform;

        Collider2D[] hits = Physics2D.OverlapCircleAll(owner.transform.position, meleeCheckRadius, enemyLayer);

        float minDist = float.MaxValue;
        Transform nearestEnemy = null;
        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(owner.transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestEnemy = hit.transform;
            }
        }

        if (nearestEnemy != null && minDist <= meleeCheckRadius)
        {
            if (_rb != null)
            {
                Destroy(_rb);
                _rb = null;
            }
            transform.SetParent(_weaponSocket);
            transform.localPosition = Vector3.zero;
            Debug.Log("근접 공격!");
        }
        else
        {
            ThrowWeapon(owner);
        }
    }

    private void ThrowWeapon(PlayerController owner)
    {
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
    }
}
