using System.Collections;
using TMPro;
using UnityEngine;

public class AI_GrenadeSkill : AI_SkillBase
{
    protected GrenadeSkillData _settings;
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

        _skillCoroutine = _controller.StartCoroutine(ThrowGrenadeRoutine(onSkillComplete));
    }

    public override void StopSkill()
    {
        if (_skillCoroutine != null)
            _controller.StopCoroutine(_skillCoroutine);
    }

    public override void SetSettings(object settings, LayerMask targetLayer, AI_Controller controller)
    {
        base.SetSettings(settings, targetLayer, controller);
        _settings = settings as GrenadeSkillData;
    }

    private IEnumerator ThrowGrenadeRoutine(System.Action onSkillComplete)
    {
        yield return new WaitForSeconds(_settings.ThrowDelay);

        if (_controller._isDead)
        {
            onSkillComplete?.Invoke();
            yield break;
        }

        ThrowGrenade();
        _movement._isUsingSkill = false;
        onSkillComplete?.Invoke();
    }

    private void ThrowGrenade()
    {
        if (_controller._detection.Player == null) return;

        Vector2 origin = _controller.transform.position;
        Vector2 target = _controller._detection.Player.position;
        Vector2 toTarget = target - origin;

        GameObject grenade = GameObject.Instantiate(
            _settings.GrenadePrefab,
            origin,
            Quaternion.identity
        );

        if (!grenade.TryGetComponent<Rigidbody2D>(out var rb)) return;

        // 탄도 계산
        float gravity = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale;
        float dx = Mathf.Abs(toTarget.x);
        float dy = toTarget.y;

        // 거리별 발사 각도 설정 (45~75도)
        float angleDeg = Mathf.Clamp(45f + dy * 2f, 45f, 90f);
        float angle = angleDeg * Mathf.Deg2Rad;

        float cosAngle = Mathf.Cos(angle);
        float sinAngle = Mathf.Sin(angle);

        float denominator = 2 * (dx * Mathf.Tan(angle) - dy) * cosAngle * cosAngle;

        if (denominator > 0)
        {
            float speed = Mathf.Sqrt(gravity * dx * dx / denominator);

            Vector2 velocity = new Vector2(
                speed * cosAngle * Mathf.Sign(toTarget.x),
                speed * sinAngle
            );

            rb.velocity = velocity;
        }
        else // 플레이어가 위에 있는 경우
        {
           rb.velocity = toTarget * 2f;
        }

        if (grenade.TryGetComponent<AI_Grenade>(out var grenadeScript))
        {
            grenadeScript.Initialize(
                _settings.Damage,
                _settings.ExplosionRadius,
                _settings.ExplosionDelay,
                _targetLayer,
                _settings.ExplosionEffect
            );
        }
    }

}
