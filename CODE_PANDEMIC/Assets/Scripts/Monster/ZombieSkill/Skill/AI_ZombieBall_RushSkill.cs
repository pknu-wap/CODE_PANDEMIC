using System.Collections;
using UnityEngine;

public class AI_ZombieBall_RushSkill : AI_SkillBase
{
    private Coroutine _rushCoroutine;
    private float _rushEndTime = -Mathf.Infinity;
    private Vector2 _rushDirection;
    private bool _isRushing = false;

    public float RushDuration { get; set; }

    public bool IsRushing => _isRushing;

    public override bool IsReady(AI_Controller controller)
    {
        return !_isRushing;
    }

    public override void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        if (!IsReady(controller)) return;

        _controller = controller;
        _rushDirection = (_detection.Player.position - _controller.transform.position).normalized;
        _isRushing = true;
        _rushEndTime = Time.time + RushDuration;
        _movement.SetSkillState(true);
        _rushCoroutine = _controller.StartCoroutine(RushRoutine(onSkillComplete));
    }

    public override void StopSkill()
    {
        if (_rushCoroutine != null && _controller != null)
        {
            _controller.StopCoroutine(_rushCoroutine);
        }
        StopRush();
    }

    private IEnumerator RushRoutine(System.Action onSkillComplete)
    {
        Rigidbody2D rb = _controller.GetComponent<Rigidbody2D>();
        while (Time.time < _rushEndTime)
        {
            if (rb != null)
            {
                rb.velocity = _rushDirection * _controller.MoveSpeed;
            }
            float rotationSpeed = _controller.MoveSpeed * 30f;
            float direction = Mathf.Sign(_rushDirection.x);
            _controller.transform.Rotate(Vector3.forward, -direction * rotationSpeed * Time.deltaTime);
            yield return null;
        }
        StopRush();
        onSkillComplete?.Invoke();
    }

    public void StopRush()
    {
        _isRushing = false;
        Rigidbody2D rb = _controller.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        _controller._movement.SetSkillState(false);
    }
}