using System.Collections;
using UnityEngine;

public class AI_ThrowSkill : ISkillBehavior
{
    protected AI_Controller _controller;
    private Coroutine _skillCoroutine;
    private float _lastSkillTime = -Mathf.Infinity;
    protected ThrowSkillData _settings;
    protected LayerMask _targetLayer;

    public void SetController(AI_Controller controller)
    {
        _controller = controller;
    }
    public virtual void SetSettings(object settings, LayerMask targetLayer, AI_Controller controller)
    {
        _controller = controller;
        _targetLayer = targetLayer;
        _settings = settings as ThrowSkillData;
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
        _controller._animator.SetBool("Attack", true);
        _skillCoroutine = _controller.StartCoroutine(ThrowRoutine(onSkillComplete));
    }

    public virtual void StopSkill()
    {
        if (_skillCoroutine != null && _controller != null)
        {
            _controller.StopCoroutine(_skillCoroutine);
        }
    }

    protected virtual IEnumerator ThrowRoutine(System.Action onSkillComplete)
    {
        if (_controller._player == null) yield break;

        var visualizer = (_controller as AI_NurseZombie)?._visualizer 
                         ?? (_controller as AI_HospitalBoss)?._syringeVisualizer;

        Vector2 origin = _controller.transform.position;
        Vector2 target = _controller._player.position;
        float width = _settings.Range;

        visualizer?.Show(origin, target, width);
        yield return new WaitForSeconds(_settings.ChargeDelay);
        visualizer?.Hide();

        ThrowSyringe((target - origin).normalized);

        _controller._aiPath.canMove = true;
        _controller._isUsingSkill = false;
        onSkillComplete?.Invoke();
    }

    protected virtual void ThrowSyringe(Vector2 direction)
    {
        GameObject prefab = (_controller as AI_NurseZombie)?._syringePrefab 
                           ?? (_controller as AI_HospitalBoss)?._syringePrefab;
                           
        float speed = _settings.SyringeSpeed;

        if (prefab == null) return;

        GameObject syringe = Object.Instantiate(prefab, _controller.transform.position, Quaternion.identity);
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
