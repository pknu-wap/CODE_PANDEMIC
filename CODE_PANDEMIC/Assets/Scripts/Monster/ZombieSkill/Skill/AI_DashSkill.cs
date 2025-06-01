using System.Collections;
using UnityEngine;

public class AI_DashSkill : ISkillBehavior
{
    protected AI_Controller _controller;
    private Coroutine _dashCoroutine;
    protected DashSkillData _settings;
    protected LayerMask _targetLayer;
    private float _lastSkillTime = -Mathf.Infinity;

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
        float distance = Vector2.Distance(start, target) + 1f;
        float duration = distance / _settings.Speed;
        float elapsed = 0f;

        // var visualizer = (_controller as AI_AthleteZombie)?._visualizer 
        //                  ?? null;
        // visualizer?.Show(start, target, _settings.Width);
    
        // visualizer?.Hide();

        Rigidbody2D rb = _controller.GetComponent<Rigidbody2D>();
        bool _hasHitPlayer = false;
        Transform playerTransform = _controller._player;
        float dashCheckRadius = 1f;

        yield return new WaitForSeconds(_settings.ChargeDelay);
        _controller._animator.SetBool("Attack", true);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (rb != null)
                rb.velocity = direction * _settings.Speed;

            RaycastHit2D hit = Physics2D.CircleCast(_controller.transform.position, _settings.Width / 2f, direction, 0.1f, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
            {
                break;
            }
            if (!_hasHitPlayer)
            {
                if (Vector2.Distance(_controller.transform.position, playerTransform.position) <= dashCheckRadius)
                {
                    _hasHitPlayer = true;
                    if (playerTransform.TryGetComponent<PlayerController>(out var player))
                    {
                        int damage = Mathf.RoundToInt(_controller.AiDamage * 0.8f);
                        player.TakeDamage(_controller.gameObject, damage);
                    }
                }
            }

            yield return null;
        }

        if (rb != null)
            rb.velocity = Vector2.zero;

        _controller._aiPath.canMove = true;
        _controller._isUsingSkill = false;
        onSkillComplete?.Invoke();
    }
}
