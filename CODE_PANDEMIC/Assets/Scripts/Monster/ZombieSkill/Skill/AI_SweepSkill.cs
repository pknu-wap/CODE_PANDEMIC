using UnityEngine;
using System.Collections;
public class AI_SweepSkill : ISkillBehavior
{
    protected AI_Controller _controller;
    protected SweepSkillData _settings;
    protected LayerMask _targetLayer;

    private Coroutine _skillCoroutine;
    private float _lastSkillTime = -Mathf.Infinity;

    public void SetController(AI_Controller controller)
    {
        _controller = controller;
    }

    public virtual bool IsReady(AI_Controller controller)
    {
        return Time.time >= _lastSkillTime + _settings.Cooldown;
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
        if (_skillCoroutine != null)
        {
            _controller.StopCoroutine(_skillCoroutine);
        }
    }

    protected virtual float GetDamageMultiplier()
    {
        return 0.5f; // 디폴트
    }
    public virtual void SetSettings(object settings, LayerMask targetLayer, AI_Controller controller)
    {
        _settings = settings as SweepSkillData;
        _targetLayer = targetLayer;
        _controller = controller;
    }


    protected virtual IEnumerator SweepRoutine(System.Action onSkillComplete)
    {
        Vector2 attackDirection = ((Vector2)_controller._player.position - (Vector2)_controller.transform.position).normalized;
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

        _controller._aiPath.canMove = true;
        _controller._isUsingSkill = false;
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
