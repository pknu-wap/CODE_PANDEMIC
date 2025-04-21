using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _playerController;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private Vector3 _weaponLocalPosition = new Vector3(-0.5f, 0f, 0f);

    // 이동 관련
    private float _walkSpeed = 5f;
    private float _runSpeed = 8f;
    private float _dashSpeed = 10f;
    private float _dashDuration = 0.1f;
    private float _dashCooldown = 0.5f;
    private Vector2 _moveInput;

    // 입력 관련
    private PlayerInput _inputAction;
    private InputAction _moveAction;
    private InputAction _runAction;
    private InputAction _dashAction;

    // 대쉬 관련
    private bool _isDashing = false;
    private float _lastDashTime = -999f;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _inputAction = new PlayerInput();

        _moveAction = _inputAction.Player.Move;
        _runAction = _inputAction.Player.Run;
        _dashAction = _inputAction.Player.Dash;
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _runAction.Enable();
        _dashAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _runAction.Disable();
        _dashAction.Disable();
    }

    private void FixedUpdate()
    {
        if (_playerController._currentState == PlayerState.Dead || _isDashing) return;

        _moveInput = _moveAction.ReadValue<Vector2>();

        if (_moveInput != Vector2.zero)
        {
            _playerController._forwardVector = _moveInput;
        }

        float speed = _runAction.IsPressed() ? _runSpeed : _walkSpeed;

        _rigidbody.MovePosition(_rigidbody.position + _moveInput * speed * Time.fixedDeltaTime);

        if (_moveInput.x != 0)
        {
            Vector3 scale = transform.localScale;
            if (_moveInput.x > 0)
                scale.x = -1;
            else if (_moveInput.x < 0)
                scale.x = 1;
            transform.localScale = scale;

            if (_playerController._weaponHolder != null)
            {
                Vector3 adjustedPosition = _weaponLocalPosition;
                adjustedPosition.x *= _spriteRenderer.flipX ? -1 : 1;
                _playerController._weaponHolder.localPosition = adjustedPosition;
            }
        }

        _playerController._currentState = _moveInput != Vector2.zero ? PlayerState.Move : PlayerState.Idle;
    }

    private void Update()
    {
        if (_playerController._currentState == PlayerState.Dead) return;

        //if (_playerController._equippedWeapon != null && Mouse.current.leftButton.wasPressedThisFrame)
        //{
        //    _playerController._equippedWeapon.Attack();
        //}

        if (_dashAction.triggered && !_isDashing && Time.time >= _lastDashTime + _dashCooldown)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private System.Collections.IEnumerator DashCoroutine()
    {
        _isDashing = true;
        _lastDashTime = Time.time;

        _playerController._currentState = PlayerState.Invincible;

        Vector2 dashDirection = _moveInput.normalized;
        float dashTime = 0f;

        while (dashTime < _dashDuration)
        {
            RaycastHit2D hit = Physics2D.Raycast(_rigidbody.position, dashDirection, _dashSpeed * Time.fixedDeltaTime, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
                break;

            _rigidbody.MovePosition(_rigidbody.position + dashDirection * _dashSpeed * Time.fixedDeltaTime);
            dashTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        _isDashing = false;

        _playerController._currentState = PlayerState.Idle;

        yield return new WaitForSeconds(0.3f);
    }
}