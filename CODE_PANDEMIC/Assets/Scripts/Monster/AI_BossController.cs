using UnityEngine;

public class AI_BossController : MonoBehaviour
{
    public float health = 100f;
    public float sweepRange = 4f;
    public float syringeRange = 4f;
    public float chargeWidth = 6f;
    public bool isBerserk = false;
    public float moveSpeed = 3f;

    private Transform player;
    private float sweepCooldown = 0f;
    private float syringeCooldown = 0f;
    private float summonCooldown = 0f;
    private float chargeCooldown = 0f;	
    
    private int baseSweepDamage = 10;
    private int baseSyringeCount = 3;

   
    private bool isAttacking = false;

    void Start()
    {
        PlayerMovement playerComponent = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if (player == null) return;

        if (isAttacking)
        {
            UpdateCooldowns();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 체력 50% 이하이면 광폭화 상태 전환
        if (health <= 50f && !isBerserk)
        {
            EnterBerserkMode();
        }

        if (distanceToPlayer <= sweepRange && sweepCooldown <= 0f)
        {
            StartCoroutine(PerformSweep());
        }
        else if (distanceToPlayer > syringeRange && syringeCooldown <= 0f)
        {
            StartCoroutine(PerformSyringeThrow());
        }
        else if (summonCooldown <= 0f)
        {
            StartCoroutine(PerformSummon());
        }
        else if (isBerserk && chargeCooldown <= 0f)
        {
            StartCoroutine(PerformCharge());
        }
        else
        {
            ChasePlayer();
        }

        UpdateCooldowns();
    }

    void UpdateCooldowns()
    {
        if (sweepCooldown > 0f) sweepCooldown -= Time.deltaTime;
        if (syringeCooldown > 0f) syringeCooldown -= Time.deltaTime;
        if (summonCooldown > 0f) summonCooldown -= Time.deltaTime;
        if (chargeCooldown > 0f) chargeCooldown -= Time.deltaTime;
    }

    void EnterBerserkMode()
    {
        isBerserk = true;
        Debug.Log("보스가 광폭화 상태로 돌입! 휩쓸기 대미지 증가, 주사기 추가 발사!");
    }

    System.Collections.IEnumerator PerformSweep()
    {
        isAttacking = true;
        int sweepDamage = isBerserk ? baseSweepDamage * 2 : baseSweepDamage;
        Debug.Log($"보스가 휩쓸기 공격을 시작! (대미지: {sweepDamage})");

        for (int i = 0; i < 10; i++)
        {
            // 실제 공격 로직(예: 충돌 판정) 실행
            Debug.Log($"휩쓸기 {i+1}/10 실행");
            yield return new WaitForSeconds(0.2f);  // 각 공격 간 간격
        }
        
        sweepCooldown = 5f;  // 쿨타임
        isAttacking = false;
    }

    System.Collections.IEnumerator PerformSyringeThrow()
    {
        isAttacking = true;
        int syringeCount = isBerserk ? baseSyringeCount + 1 : baseSyringeCount;
        Debug.Log($"보스가 주사기를 {syringeCount}개 던지기 시작!");

        // 주사기 던지기 
        yield return new WaitForSeconds(0.5f);  
        
        syringeCooldown = 8f;
        isAttacking = false;
    }

    System.Collections.IEnumerator PerformSummon()
    {
        isAttacking = true;
        Debug.Log("보스가 좀비 소환을 시작!");
        
        // 좀비 소환
        yield return new WaitForSeconds(0.5f);  
        
        summonCooldown = 12f;
        isAttacking = false;
    }

    System.Collections.IEnumerator PerformCharge()
    {
        isAttacking = true;
        Debug.Log("보스가 돌진 공격을 시작!");
        
        // 돌진 공격 
        yield return new WaitForSeconds(0.5f);  
        
        chargeCooldown = 15f;
        isAttacking = false;
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        Debug.Log("보스가 플레이어에게 접근 중...");
    }
}
