using System.Collections;
using UnityEngine;

public class AI_DashSkill : ISkillBehavior
{
    protected AI_Controller _controller;
    private Coroutine _dashCoroutine;
    protected DashSkillData _settings;
    protected LayerMask _targetLayer;
    private float _lastSkillTime = -Mathf.Infinity;
    protected float _baseAnimationDuration = 0.5f; // 기본 애니메이션 길이

    public void SetController(AI_Controller controller)
    {
        _controller = controller;
    }

    public virtual void SetSettings(object settings, LayerMask targetLayer, AI_Controller controller)
    {
        _controller = controller;
        _targetLayer = targetLayer;
        _settings = settings as DashSkillData;
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
        _dashCoroutine = _controller.StartCoroutine(DashRoutine(onSkillComplete));
    }

    public virtual void StopSkill()
    {
        if (_dashCoroutine != null && _controller != null)
        {
            _controller.StopCoroutine(_dashCoroutine);
        }
    }

    protected virtual IEnumerator DashRoutine(System.Action onSkillComplete)
    {
        Vector2 start = _controller.transform.position;
        Vector2 target = _controller._player.position;

        Vector2 direction = (target - start).normalized;
        float distanceToPlayer = Vector2.Distance(start, target);
        
        float dashDistance;
        
        if (distanceToPlayer <= _controller.DetectionRange / 2)
        {
            dashDistance = _controller.DetectionRange / 2;
            _controller._animator.speed = 1f;
        }
        else
        {
            dashDistance = distanceToPlayer;
            float tempDuration = dashDistance / _settings.Speed;
            _controller._animator.speed = _baseAnimationDuration / tempDuration;
        }

        float duration = dashDistance / _settings.Speed;
        float elapsed = 0f;

        yield return CoroutineHelper.WaitForSeconds(_settings.ChargeDelay);

        _controller._animator.SetBool("Attack", true);
        Rigidbody2D rb = _controller.GetComponent<Rigidbody2D>();
        bool _hasHitPlayer = false;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (rb != null)
                rb.velocity = direction * _settings.Speed;

            if (!_hasHitPlayer && Vector2.Distance(_controller.transform.position, _controller._player.position) <= 1f)
            {
                _hasHitPlayer = true;
                if (_controller._player.TryGetComponent<PlayerController>(out var player))
                {
                    int damage = Mathf.RoundToInt(_controller.AiDamage * 0.8f);
                    player.TakeDamage(_controller.gameObject, damage);
                }
            }

            RaycastHit2D hit = Physics2D.CircleCast(_controller.transform.position, _settings.Width / 2f, direction, 0.1f, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
                break;

            yield return null;
        }

        if (rb != null)
            rb.velocity = Vector2.zero;

        _controller._animator.speed = 1f;
        _controller._aiPath.canMove = true;
        _controller._isUsingSkill = false;
        onSkillComplete?.Invoke();
    }
}
