using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AI_SweepSkill : ISkillBehavior
{
    private Coroutine _skillCoroutine;
    private float _lastSkillTime = -Mathf.Infinity;
    private AI_DoctorZombie _currentDoctor;
    
    public void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {

        if (!IsReady(controller))
        {
            Debug.Log($"[SweepSkill] 쿨타임 중, 남은 시간: {_currentDoctor.SkillCooldown - (Time.time - _lastSkillTime):F2}초");
            onSkillComplete?.Invoke();
            return;
        }

        _lastSkillTime = Time.time;
        _skillCoroutine = controller.StartCoroutine(SweepRoutine(controller as AI_DoctorZombie, onSkillComplete));
    }

    public void StopSkill()
    {
    }

    private IEnumerator SweepRoutine(AI_DoctorZombie doctor, System.Action onSkillComplete)
    {
        var aiPath = doctor._aiPath;
        bool originalCanMove = aiPath.canMove;
        aiPath.canMove = false;

        yield return new WaitForSeconds(doctor.SkillChargeDelay);

        for (int i = 0; i < doctor.SweepCount; i++)
        {
            Vector2 attackDirection = (doctor.Player != null)
                ? ((Vector2)doctor.Player.position - (Vector2)doctor.transform.position).normalized
                : doctor.transform.up;
            DoSweepAttack(doctor, attackDirection);
            yield return new WaitForSeconds(doctor.SweepInterval);
        }

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
            if (Vector2.Angle(forward, toTarget) <= doctor.SweepAngle * 0.5f)
            {
                float damage = doctor.AiDamage * 0.5f;
                Debug.Log($"[SweepSkill] {doctor.AIName} 타격: {hit.name}에게 {damage} 데미지");
            }
        }
    }

    public bool IsReady(AI_Controller controller)
    {
        if (_currentDoctor == null)
            _currentDoctor = controller as AI_DoctorZombie;
        if (_currentDoctor != null)
            return Time.time >= _lastSkillTime + _currentDoctor.SkillCooldown;

        return false;
    }
}
