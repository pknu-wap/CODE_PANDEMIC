using System.Collections;
using UnityEngine;

public class AI_SweepSkill : ISkillBehavior
{
    private Coroutine _skillCoroutine;
    private float _lastSkillTime = -Mathf.Infinity;  // 쿨타임 체크용

    public void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        var doctor = controller as AI_DoctorZombie;
        if (doctor == null)
        {
            onSkillComplete?.Invoke();
            return;
        }
        
        // 쿨타임 체크: 15초 경과 후에만 실행
        if (Time.time < _lastSkillTime + doctor.SweepCooldown)
        {
            Debug.Log($"[SweepSkill] 쿨타임 중, 남은 시간: {doctor.SweepCooldown - (Time.time - _lastSkillTime):F2}초");
            onSkillComplete?.Invoke();
            return;
        }
        _lastSkillTime = Time.time;
        _skillCoroutine = controller.StartCoroutine(SweepRoutine(doctor, onSkillComplete));
    }

    public void StopSkill()
    {
        // 필요한 경우 스킬 중단 로직 구현 (여기서는 별도 처리하지 않음)
    }

    private IEnumerator SweepRoutine(AI_DoctorZombie doctor, System.Action onSkillComplete)
    {
        // 스킬 실행 동안 이동 금지: AIPath 컴포넌트 사용
        var aiPath = doctor._aiPath; 
        bool originalCanMove = aiPath.canMove;
        aiPath.canMove = false;

        // 스킬 루틴: SweepCount만큼 반복 실행 (플레이어가 범위 밖이어도 루틴은 끝까지 진행)
        for (int i = 0; i < doctor.SweepCount; i++)
        {
            // 공격 방향: 항상 현재 플레이어 위치를 기준으로 계산
            Vector2 attackDirection;
            if (doctor.Player != null)
                attackDirection = ((Vector2)doctor.Player.position - (Vector2)doctor.transform.position).normalized;
            else
                attackDirection = doctor.transform.up; // fallback

            DoSweepAttack(doctor, attackDirection);
            yield return new WaitForSeconds(doctor.SweepInterval);
        }

        // 스킬 실행 후 원래 이동 상태로 복구
        aiPath.canMove = originalCanMove;
        onSkillComplete?.Invoke();
    }

    private void DoSweepAttack(AI_DoctorZombie doctor, Vector2 forward)
    {
        Vector2 origin = doctor.transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, doctor.SweepRange, doctor.TargetLayer);

        foreach (var hit in hits)
        {
            Vector2 toTarget = ((Vector2)hit.transform.position - origin).normalized;
            // 부채꼴 내인지 확인: forward 기준 반경 SweepAngle/2
            if (Vector2.Angle(forward, toTarget) <= doctor.SweepAngle * 0.5f)
            {
                float damage = doctor.AiDamage * 0.5f;  // 공격력의 0.5배
                Debug.Log($"[SweepSkill] {doctor.AIName} 타격: {hit.name}에게 {damage} 데미지");
                
            }
        }
    }
}
