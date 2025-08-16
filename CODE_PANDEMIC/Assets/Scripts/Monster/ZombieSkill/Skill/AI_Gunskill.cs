using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Gunskill : AI_SkillBase
{
    protected GunSkillData _settings;

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
        _movement._isUsingSkill = true;
        _controller._movement.StopMoving();

        _skillCoroutine = _controller.StartCoroutine(GunRoutine(onSkillComplete));
    }

    public override void StopSkill()
    {
        if (_skillCoroutine != null)
        {
            _controller.StopCoroutine(_skillCoroutine);
        }
    }

    public override void SetSettings(object settings, LayerMask targetLayer, AI_Controller controller)
    {
        base.SetSettings(settings, targetLayer, controller);
        _settings = settings as GunSkillData;
    }

    protected virtual IEnumerator GunRoutine(System.Action onSkillComplete)
{
    if (_controller._detection.Player == null)
    {
        onSkillComplete?.Invoke();
        yield break;
    }

    Vector2 origin = _controller.transform.position;
    Vector2 toPlayer = ((Vector2)_controller._detection.Player.position - origin).normalized;
    float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

    _controller._animator.SetBool("Attack", true);

    int shotCount = _settings.StrikeCount;
    float angleStep = 60f / (shotCount - 1);
    float startAngle = baseAngle - 30f;

    for (int i = 0; i < shotCount; i++)
    {
        if (_controller._isDead)
        {
            onSkillComplete?.Invoke();
            yield break;
        }

        float currentAngle = startAngle + i * angleStep;
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        FireBullet(origin, direction);

        yield return new WaitForSeconds(_settings.FireRate);
    }
        _lastSkillTime = Time.time;
        _controller._movement.ChasePlayer();
        _movement._isUsingSkill = false;
        onSkillComplete?.Invoke();
}

protected virtual void FireBullet(Vector2 origin, Vector2 direction)
{
    GameObject bullet = GameObject.Instantiate(
        _settings.BulletPrefab, 
        origin, 
        Quaternion.LookRotation(Vector3.forward, direction)
    );

    if (bullet.TryGetComponent<AI_SoliderBullet>(out var bulletScript))
    {
        bulletScript.Initialize(direction, _settings.BulletSpeed, _settings.Damage, _settings.Range, _targetLayer);
    }
}

}

