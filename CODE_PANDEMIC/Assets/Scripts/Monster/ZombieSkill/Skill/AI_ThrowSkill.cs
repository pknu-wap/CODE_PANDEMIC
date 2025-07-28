using System.Collections;
using UnityEngine;

public class AI_ThrowSkill : AI_SkillBase
{
    private Coroutine _skillCoroutine;
    private float _lastSkillTime = -Mathf.Infinity;
    protected ThrowSkillSettings _settings;

    public override void SetSettings(object settings, LayerMask targetLayer, AI_Controller controller)
    {
        base.SetSettings(settings, targetLayer, controller);
        _settings = settings as ThrowSkillSettings;
    }

    public override bool IsReady(AI_Controller controller)
    {
        return Time.time >= _lastSkillTime + _settings.Data.Cooldown;
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
        _controller._animator.SetBool("Attack", true);
        _skillCoroutine = _controller.StartCoroutine(ThrowRoutine(onSkillComplete));
    }

    public override void StopSkill()
    {
        if (_skillCoroutine != null)
        {
            _controller.StopCoroutine(_skillCoroutine);
            _settings.Visualizer?.Hide();
        }
    }

    protected virtual IEnumerator ThrowRoutine(System.Action onSkillComplete)
    {
        if (_controller._detection.Player == null) yield break;

        Vector2 origin = _controller.transform.position;
        Vector2 target = _controller._detection.Player.position;
        float width = _settings.Data.Range;

        _settings.Visualizer?.Show(origin, target, width);
        yield return new WaitForSeconds(_settings.Data.ChargeDelay);
        _settings.Visualizer?.Hide();

        ThrowSyringe((target - origin).normalized);

        _controller._movement.ChasePlayer();
        _movement._isUsingSkill = false;
        onSkillComplete?.Invoke();
    }

    protected virtual void ThrowSyringe(Vector2 direction)
    {
        GameObject prefab = _settings.SyringePrefab;

        float speed = _settings.Data.SyringeSpeed;

        if (prefab == null) return;
        Transform parent = _controller?.transform.parent;
        GameObject syringe = Object.Instantiate(prefab, _controller.transform.position, Quaternion.identity, parent);
        Rigidbody2D rb = syringe.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        syringe.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (syringe.TryGetComponent(out Projectile proj))
        {
            proj.SetOwner(_controller);
        }
    }
}
