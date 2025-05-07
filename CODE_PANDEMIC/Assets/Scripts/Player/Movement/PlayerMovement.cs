using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private float _walkSpeed = 3.5f;
    private float _runSpeed = 4.5f;
    private float _dashSpeed = 8f;
    private float _dashDuration = 0.3f;
    private float _dashCooldown = 0.5f;

    private bool _isDashing = false;
    public bool IsDashing => _isDashing;
    private float _lastDashTime = -999f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 input, bool isRunning)
    {
        if (_isDashing) return;

        float speed = isRunning ? _runSpeed : _walkSpeed;
        _rigidbody.MovePosition(_rigidbody.position + input * speed * Time.fixedDeltaTime);

        // 방향 전환 (flip)
        if (input.x != 0)
        {
            float scaleX = transform.localScale.x;
            if ((input.x > 0 && scaleX > 0) || (input.x < 0 && scaleX < 0))
            {
                scaleX *= -1;
                transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    public void TryDash(Vector2 direction)
    {
        if (_isDashing || Time.time < _lastDashTime + _dashCooldown) return;
        StartCoroutine(DashCoroutine(direction));
    }

    private IEnumerator DashCoroutine(Vector2 dir)
    {
        _isDashing = true;
        _lastDashTime = Time.time;

        float dashTime = 0f;
        Vector2 dashDir = dir.normalized;

        while (dashTime < _dashDuration)
        {
            RaycastHit2D hit = Physics2D.Raycast(_rigidbody.position, dashDir, _dashSpeed * Time.fixedDeltaTime, LayerMask.GetMask("Wall"));
            if (hit.collider != null) break;

            _rigidbody.MovePosition(_rigidbody.position + dashDir * _dashSpeed * Time.fixedDeltaTime);
            dashTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        _isDashing = false;
    }

    public void StopImmediately()
    {
        _rigidbody.velocity = Vector2.zero;
    }
}
