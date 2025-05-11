using System.Collections;
using UnityEngine;

public class AI_DashSkill : ISkillBehavior
{
    protected AI_Controller _controller;
    private Coroutine _dashCoroutine;
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
    Vector2 dashDirection = (_controller._player.position - _controller.transform.position).normalized;
    float startTime = Time.time;
    float endTime = startTime + DashDuration;

    _controller._aiPath.canMove = false;

    Rigidbody2D rb = _controller.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.velocity = dashDirection * DashSpeed;
    }

    bool _hasHitPlayer = false;
    Transform playerTransform = _controller._player;
    float dashCheckRadius = 1.0f;

    while (Time.time < endTime)
    {
        if (!_hasHitPlayer){
            float distance = Vector2.Distance(_controller.transform.position, playerTransform.position);
            if (distance <= dashCheckRadius)
            {
                _hasHitPlayer = true;

                if (playerTransform.TryGetComponent<PlayerStatus>(out var playerStatus))
                {
                    int damage = Mathf.RoundToInt(_controller.AiDamage * 1.5f);
                    Debug.Log($"[AI_DashSkill] hit {playerTransform.gameObject.name} for {damage} damage.");
                    // playerStatus.OnDamaged(_controller.gameObject, damage);
                }
            }
        }

        yield return null;
    }


    rb.velocity = Vector2.zero;
    Debug.Log($"[AI_DashSkill] {rb.velocity} stopped.");

    _controller._aiPath.canMove = true;
    _controller._isUsingSkill = false;
    onSkillComplete?.Invoke();
}



    // === Overridable Properties ===

    protected virtual float Cooldown => 20f;
    protected virtual float DashDuration => 0.3f;
    protected virtual float DashSpeed => 40f; // DashRange / DashDuration (e.g., 12 / 0.3 = 40)
    protected virtual float DashRange => 12f;
    protected virtual float DashWidth => 5f;
}
