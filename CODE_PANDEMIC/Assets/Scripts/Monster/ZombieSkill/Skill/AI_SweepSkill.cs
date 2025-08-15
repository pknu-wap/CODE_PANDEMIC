using UnityEngine;
using System.Collections;
public class AI_SweepSkill : AI_SkillBase
{
    protected SweepSkillData _settings;

    private Coroutine _skillCoroutine;
    private float _lastSkillTime = -Mathf.Infinity;

    public override bool IsReady(AI_Controller controller)
    {
        return Time.time >= _lastSkillTime + _settings.Cooldown;
    }

    public override void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        if (!IsReady(controller))
        {
            onSkillComplete?.Invoke();
            return;
        }

        _controller = controller;
        _lastSkillTime = Time.time;
        _movement._isUsingSkill = true;
        _controller._movement.StopMoving();

        _skillCoroutine = _controller.StartCoroutine(SweepRoutine(onSkillComplete));
    }

    public override void StopSkill()
    {
        if (_skillCoroutine != null)
        {
            _controller.StopCoroutine(_skillCoroutine);
        }
    }

    protected virtual float GetDamageMultiplier()
    {
        return 0.5f; // 디폴트
    }
    public override void SetSettings(object settings, LayerMask targetLayer, AI_Controller controller)
    {
        base.SetSettings(settings, targetLayer, controller);
        _settings = settings as SweepSkillData;
    }


    protected virtual IEnumerator SweepRoutine(System.Action onSkillComplete)
    {
        Vector2 attackDirection = ((Vector2)_controller._detection.Player.position - (Vector2)_controller.transform.position).normalized;
        _controller._animator.SetBool("Attack", true);
        for (int i = 0; i < _settings.Count; i++)
        {
            if (_controller._isDead)
            {
                onSkillComplete?.Invoke();
                yield break;
            }
            DoSweepAttack(attackDirection);
            yield return new WaitForSeconds(_settings.Interval);
        }

        _controller._movement.ChasePlayer();
        _movement._isUsingSkill = false;
        onSkillComplete?.Invoke();
    }

    protected virtual void DoSweepAttack(Vector2 forward)
    {
        Vector2 origin = _controller.transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, _settings.Range, _targetLayer);

        foreach (var hit in hits)
        {
            Vector2 toTarget = ((Vector2)hit.transform.position - origin).normalized;
            if (Vector2.Angle(forward, toTarget) <= _settings.Angle * 0.5f)
            {
                float damage = _controller.AiDamage * GetDamageMultiplier();

                if (hit.TryGetComponent<PlayerController>(out var player))
                {
                    player.TakeDamage(_controller.gameObject, damage);
                }
            }
        }
    }
}
