using UnityEngine;
using System.Collections;
using System;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private float _walkSpeed ;
    private float _runSpeed ;
    private float _dashSpeed ;
    private float _dashDuration;
    private float _dashCooldown ;

    private bool _isDashing = false;
    public bool IsDashing => _isDashing;
    private float _lastDashTime = -999f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        PlayerStat data = Managers.Game.PlayerStat;
        _walkSpeed = data.BaseSpeed;
        _runSpeed = data.RunSpeed;
        _dashSpeed = data.DashSpeed;    
        _dashDuration = data.DashDuration;
        _dashCooldown = data.DashCoolDown;
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
        _rigidbody.MovePosition(_rigidbody.position + input * speed * Time.fixedDeltaTime);

        // 방향 전환
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
    private void OnStatUpdated(object obj)
    {
        PlayerStat stat = Managers.Game.PlayerStat;
        _walkSpeed = stat.BaseSpeed;
        Debug.Log(_walkSpeed);
        _runSpeed = stat.RunSpeed;
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
