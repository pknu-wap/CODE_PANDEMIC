using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private float _walkSpeed;
    private float _runSpeed;
    private float _dashSpeed;
    private float _dashDuration;
    private float _dashCooldown;
    private float _lastDashTime = -999f;
    private bool _isDashing = false;

    public bool IsDashing => _isDashing;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        UpdateStats();
    }

    private void OnEnable()
    {
        Managers.Event.Subscribe("StatUpdated", OnStatUpdated);
    }

    private void OnDisable()
    {
        Managers.Event.Unsubscribe("StatUpdated", OnStatUpdated);
    }

    public void Move(Vector2 input, bool isRunning)
    {
        if (_isDashing) return;

        float speed = isRunning ? _runSpeed : _walkSpeed;
        Vector2 movement = input * speed * Time.fixedDeltaTime;

        _rigidbody.MovePosition(_rigidbody.position + movement);

        HandleFlip(input.x);
    }

    private void HandleFlip(float inputX)
    {
        if (inputX == 0) return;

        float scaleX = transform.localScale.x;

        bool needsFlip = (inputX > 0 && scaleX > 0) || (inputX < 0 && scaleX < 0);

        if (needsFlip)
        {
            scaleX *= -1;
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnStatUpdated(object obj)
    {
        UpdateStats();
    }

    private void UpdateStats()
    {
        PlayerStat stat = Managers.Game.PlayerStat;

        _walkSpeed = stat.BaseSpeed;
        _runSpeed = stat.RunSpeed;
        _dashSpeed = stat.DashSpeed;
        _dashDuration = stat.DashDuration;
        _dashCooldown = stat.DashCoolDown;

        Debug.Log($"Stats Updated: WalkSpeed={_walkSpeed}, RunSpeed={_runSpeed}");
    }
    public void TryDash(Vector2 direction)
    {
        if (_isDashing || Time.time < _lastDashTime + _dashCooldown) return;

        StartCoroutine(DashCoroutine(direction));
    }

    private IEnumerator DashCoroutine(Vector2 direction)
    {
        _isDashing = true;
        _lastDashTime = Time.time;

        float dashTime = 0f;
        Vector2 dashDir = direction.normalized;

        while (dashTime < _dashDuration)
        {
            if (IsWallAhead(dashDir)) break;

            _rigidbody.MovePosition(_rigidbody.position + dashDir * _dashSpeed * Time.fixedDeltaTime);

            dashTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        _isDashing = false;
    }

    private bool IsWallAhead(Vector2 dashDir)
    {
        float distance = _dashSpeed * Time.fixedDeltaTime;
        RaycastHit2D hit = Physics2D.Raycast(_rigidbody.position, dashDir, distance, LayerMask.GetMask("Wall"));
        return hit.collider != null;
    }

    public void StopImmediately()
    {
        _rigidbody.velocity = Vector2.zero;
    }
}
