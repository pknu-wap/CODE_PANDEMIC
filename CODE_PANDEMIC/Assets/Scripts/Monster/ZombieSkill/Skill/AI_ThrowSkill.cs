using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class AI_ThrowSkill : ISkillBehavior
{
    private Coroutine _skillCoroutine;
    private float _lastSkillTime = -Mathf.Infinity;
    private AI_NurseZombie _currentNurse;
    
    public void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        if (!IsReady(controller))
        {
            onSkillComplete?.Invoke();
            return;
        }

        _lastSkillTime = Time.time;
        _skillCoroutine = controller.StartCoroutine(SweepRoutine(controller as AI_NurseZombie, onSkillComplete));
    }

    public void StopSkill()
    {

    }

    private IEnumerator SweepRoutine(AI_NurseZombie nurse, System.Action onSkillComplete)
    {
        var aiPath = nurse._aiPath;
        bool originalCanMove = aiPath.canMove;
        aiPath.canMove = false;

        Vector2 attackDirection = (nurse.Player != null)
            ? ((Vector2)nurse.Player.position - (Vector2)nurse.transform.position).normalized
            : nurse.transform.up;

        if (nurse.ThrowVisualizer != null)
        {
            nurse.ThrowVisualizer.transform.position = nurse.transform.position;
            nurse.ThrowVisualizer.Show(nurse.SkillChargeDelay);
        }

        yield return new WaitForSeconds(nurse.SkillChargeDelay);

        for (int i = 0; i < nurse.SweepCount; i++)
        {
            DoSweepAttack(nurse, attackDirection);
            yield return new WaitForSeconds(nurse.SweepInterval);
        }

        nurse.ThrowVisualizer?.Hide();
        aiPath.canMove = true;
        onSkillComplete?.Invoke();
    }

    private void DoSweepAttack(AI_NurseZombie nurse, Vector2 forward)
    {
        Vector2 origin = nurse.transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, nurse.SweepRange, nurse.TargetLayer);

        foreach (var hit in hits)
        {
            Vector2 toTarget = ((Vector2)hit.transform.position - origin).normalized;
            if (Vector2.Angle(forward, toTarget) <= nurse.SweepAngle * 0.5f)
            {
                float damage = nurse.AiDamage * 0.5f;
                Debug.Log($"[SweepSkill] {nurse.AIName} 타격: {hit.name}에게 {damage} 데미지");
            }
        }
    }

    public bool IsReady(AI_Controller controller)
    {
        if (_currentNurse == null)
            _currentNurse = controller as AI_NurseZombie;
        if (_currentNurse != null)
            return Time.time >= _lastSkillTime + _currentNurse.SkillCooldown;

        return false;
    }
}
