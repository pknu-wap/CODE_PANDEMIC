using System.Collections;
using System.IO;
using UnityEngine;

public class AI_ThrowSkill : ISkillBehavior
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
    var visualizer = (_controller as AI_NurseZombie)?._syringeVisualizer 
                     ?? (_controller as AI_HospitalBoss)?._syringeVisualizer;
        if (_controller._player == null){yield break;}
        Vector2 origin = _controller.transform.position;
        Vector2 target = _controller._player.position;
        float width = _nurseZombie?.SyringeRange ?? _hospitalBoss?.SyringeRange ?? 5f;
        float height = 0.5f;
        visualizer.Show(origin, target, width, height);

    yield return new WaitForSeconds(ChargeDelay);
    if (visualizer != null)
    {
        visualizer.Hide();
    }

    ThrowSyringe((target - origin).normalized);

    _controller._aiPath.canMove = true;
    onSkillComplete?.Invoke();
    _controller._isUsingSkill = false;
}

    protected virtual void ThrowSyringe(Vector2 direction)
    {
        GameObject prefab = GetSyringePrefab();
        float speed = GetSyringeSpeed();

        if (prefab == null) return;

        GameObject syringe = Object.Instantiate(prefab, _controller.transform.position, Quaternion.identity);
        Rigidbody2D rb = syringe.GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.velocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        syringe.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (syringe.TryGetComponent<Projectile>(out var proj))
        {
            proj.SetOwner(_controller);
        }
    }
    protected AI_NurseZombie _nurseZombie => _controller as AI_NurseZombie;
    protected AI_HospitalBoss _hospitalBoss => _controller as AI_HospitalBoss; 
    protected virtual float Cooldown => _nurseZombie?.SkillCooldown ?? _hospitalBoss?.ThrowCooldown ?? 15f;
    protected virtual float ChargeDelay => _nurseZombie?.SkillChargeDelay ?? _hospitalBoss?.SkillChargeDelay ?? 0.5f;
    protected virtual GameObject GetSyringePrefab() =>
        _nurseZombie?._syringePrefab ?? _hospitalBoss?._syringePrefab;

    protected virtual float GetSyringeSpeed() =>
        _nurseZombie?.SyringeSpeed ?? _hospitalBoss?.SyringeSpeed ?? 10f;
}
