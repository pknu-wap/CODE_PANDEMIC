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
    Vector2 start = _controller.transform.position;
    Vector2 target = _controller._player.position;

    Vector2 direction = (target - start).normalized;
    float distance = Vector2.Distance(start, target) + 2f;
    float duration = distance / DashSpeed;

    float elapsed = 0f;
    _controller._aiPath.canMove = false;

    Rigidbody2D rb = _controller.GetComponent<Rigidbody2D>();
    bool _hasHitPlayer = false;
    Transform playerTransform = _controller._player;
    float dashCheckRadius = 1.0f;

    yield return new WaitForSeconds(DashDuration);

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        if (rb != null)
        {
            rb.velocity = direction * DashSpeed;
        }

        if (!_hasHitPlayer)
        {
            float currentDistance = Vector2.Distance(_controller.transform.position, playerTransform.position);
            if (currentDistance <= dashCheckRadius)
            {
                _hasHitPlayer = true;

                if (playerTransform.TryGetComponent<PlayerStatus>(out var playerStatus))
                {
                    int damage = Mathf.RoundToInt(_controller.AiDamage * 0.8f);
                    playerStatus.OnDamaged(_controller.gameObject, damage);
                }
            }
        }

        yield return null;
    }

    if (rb != null)
    {
        rb.velocity = Vector2.zero;
    }

    _controller._aiPath.canMove = true;
    _controller._isUsingSkill = false;
    onSkillComplete?.Invoke();
}



    protected AI_AthleteZombie _athleteZombie => _controller as AI_AthleteZombie;
    protected AI_HospitalBoss _hospitalBoss => _controller as AI_HospitalBoss; 

    protected virtual float Cooldown => _athleteZombie?.SkillCooldown ?? _hospitalBoss?.DashCooldown ?? 20f;
    protected virtual float DashDuration => _athleteZombie?.SkillChargeDelay ?? _hospitalBoss?.DashDuration ?? 1f;
    protected virtual float DashSpeed => _athleteZombie?.DashSpeed ?? _hospitalBoss?.DashSpeed ?? 40f; // DashRange / DashDuration (e.g., 12 / 0.3 = 40)
    protected virtual float DashWidth => _athleteZombie?.DashWidth ?? _hospitalBoss?.DashWidth ?? 1f;
}
