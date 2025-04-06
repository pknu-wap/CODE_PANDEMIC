using System.Collections;
using UnityEngine;

public class AI_BossController : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private bool _isBerserk = false;

    [SerializeField] private float _sweepRange = 4f;
    [SerializeField] private float _syringeRange = 4f;
    [SerializeField] private float _chargeWidth = 6f;

    private float _sweepCooldown = 0f;
    private float _syringeCooldown = 0f;
    private float _summonCooldown = 0f;
    private float _chargeCooldown = 0f;

    private int _baseSweepDamage = 10;
    private int _baseSyringeCount = 3;
    private bool _isAttacking = false;

    private Transform _player;

    private void Start()
    {
        var PlayerController = FindObjectOfType<PlayerController>();
        if (PlayerController != null)
        {
            _player = PlayerController.transform;
        }
        else
        {
            Debug.LogError("플레이어 없음");
        }
    }

    private void Update()
    {
        if (_player == null) return;

        if (_isAttacking)
        {
            UpdateCooldowns();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (_health <= 50f && !_isBerserk)
        {
            EnterBerserkMode();
        }

        if (distanceToPlayer <= _sweepRange && _sweepCooldown <= 0f)
        {
            StartCoroutine(PerformSweep());
        }
        else if (distanceToPlayer > _syringeRange && _syringeCooldown <= 0f)
        {
            StartCoroutine(PerformSyringeThrow());
        }
        else if (_summonCooldown <= 0f)
        {
            StartCoroutine(PerformSummon());
        }
        else if (_isBerserk && _chargeCooldown <= 0f)
        {
            StartCoroutine(PerformCharge());
        }
        else
        {
            ChasePlayer();
        }

        UpdateCooldowns();
    }

    private void UpdateCooldowns()
    {
        if (_sweepCooldown > 0f) _sweepCooldown -= Time.deltaTime;
        if (_syringeCooldown > 0f) _syringeCooldown -= Time.deltaTime;
        if (_summonCooldown > 0f) _summonCooldown -= Time.deltaTime;
        if (_chargeCooldown > 0f) _chargeCooldown -= Time.deltaTime;
    }

    private void EnterBerserkMode()
    {
        _isBerserk = true;
        Debug.Log("Berserker");
    }

    private IEnumerator PerformSweep()
    {
        _isAttacking = true;
        int sweepDamage = _isBerserk ? _baseSweepDamage * 2 : _baseSweepDamage;
        Debug.Log($"Damage: {sweepDamage})");

        for (int i = 0; i < 10; i++)
        {
            Debug.Log($"Swipe {i + 1}/10 실행");
            yield return new WaitForSeconds(0.2f);
        }

        _sweepCooldown = 5f;
        _isAttacking = false;
    }

    private IEnumerator PerformSyringeThrow()
    {
        _isAttacking = true;
        int syringeCount = _isBerserk ? _baseSyringeCount + 1 : _baseSyringeCount;
        Debug.Log($"{syringeCount}개 투척");

        yield return new WaitForSeconds(0.5f);

        _syringeCooldown = 8f;
        _isAttacking = false;
    }

    private IEnumerator PerformSummon()
    {
        _isAttacking = true;
        Debug.Log("zombie summon");

        yield return new WaitForSeconds(0.5f);

        _summonCooldown = 12f;
        _isAttacking = false;
    }

    private IEnumerator PerformCharge()
    {
        _isAttacking = true;
        Debug.Log("Charge Attack");

        yield return new WaitForSeconds(0.5f);

        _chargeCooldown = 15f;
        _isAttacking = false;
    }

    private void ChasePlayer()
    { 
        // 일단 A* 적용 안함(A* 없어도 맵이 충분히 잘 따라 올 수 있음)
        Vector3 direction = (_player.position - transform.position).normalized;
        transform.position += direction * _moveSpeed * Time.deltaTime;
        Debug.Log("chaseplayer");
    }
}
