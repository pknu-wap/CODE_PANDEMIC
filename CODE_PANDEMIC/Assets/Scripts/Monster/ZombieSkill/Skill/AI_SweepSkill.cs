using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SweepSkill : ISkillBehavior
{
    protected AI_Controller _controller;
    private Coroutine _skillCoroutine;
    private float _lastSkillTime = -Mathf.Infinity;

    public void SetController(AI_Controller controller)
    {
        _controller = controller;
    }

    public virtual bool IsReady(AI_Controller controller)
    {
        return Time.time >= _lastSkillTime + Cooldown;
    }

    public virtual void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        if (!IsReady(controller))
        {
            onSkillComplete?.Invoke();
            return;
        }

        _controller = controller;
        _lastSkillTime = Time.time;
        _controller._isUsingSkill = true;
        _controller._aiPath.canMove = false;

        _skillCoroutine = _controller.StartCoroutine(SweepRoutine(onSkillComplete));
    }

    public virtual void StopSkill()
    {
        if (_skillCoroutine != null && _controller != null)
        {
            _controller.StopCoroutine(_skillCoroutine);
        }
    }
    protected AI_DoctorZombie _doctorZombie => _controller as AI_DoctorZombie;
    protected AI_HospitalBoss _hospitalBoss => _controller as AI_HospitalBoss;
    protected virtual float Cooldown => _doctorZombie?.SkillCooldown ?? _hospitalBoss?.SweepCooldown ?? 15f;
    protected virtual float ChargeDelay => _doctorZombie?.SkillChargeDelay ?? _hospitalBoss?.SkillChargeDelay ?? 0.5f;
    protected virtual int SweepCount => _doctorZombie?.SweepCount ?? _hospitalBoss?.SweepCount ?? 5;
    protected virtual float SweepInterval => _doctorZombie?.SweepInterval ?? _hospitalBoss?.SweepInterval ?? 0.5f;
    protected virtual float SweepRange => _doctorZombie?.SweepRange ?? _hospitalBoss?.SweepRange ?? 4f;
    protected virtual float SweepAngle => _doctorZombie?.SweepAngle ?? _hospitalBoss?.SweepAngle ?? 90f;
    protected virtual LayerMask TargetLayer => LayerMask.GetMask("Player");

    protected virtual IEnumerator SweepRoutine(System.Action onSkillComplete)
    {
        AI_SweepVisualizer visualizer = (_controller as AI_DoctorZombie)?._sweepVisualizer;
        Vector2 attackDirection = (_controller._player != null)
            ? ((Vector2)_controller._player.position - (Vector2)_controller.transform.position).normalized
            : _controller.transform.up;

        if (visualizer != null)
        {
            visualizer.transform.position = _controller.transform.position;
            visualizer.Show(attackDirection, SweepAngle, SweepRange, ChargeDelay);
        }
        yield return new WaitForSeconds(ChargeDelay);
        

        for (int i = 0; i < SweepCount; i++)
        {
            DoSweepAttack(attackDirection);
            yield return new WaitForSeconds(SweepInterval);
        }
        visualizer?.Hide();
        _controller._aiPath.canMove = true;
        _controller._isUsingSkill = false;
        onSkillComplete?.Invoke();
    }

    protected virtual void DoSweepAttack(Vector2 forward)
    {
        Vector2 origin = _controller.transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, SweepRange, TargetLayer);
        foreach (var hit in hits)
        {
            Vector2 toTarget = ((Vector2)hit.transform.position - origin).normalized;
            if (Vector2.Angle(forward, toTarget) <= SweepAngle * 0.5f)
            {
                float damage = _controller.AiDamage * 0.5f;

                if (hit.TryGetComponent<PlayerStatus>(out var player))
                {
                    player.OnDamaged(_controller.gameObject, damage);
                }
            }
        }
    }
}
