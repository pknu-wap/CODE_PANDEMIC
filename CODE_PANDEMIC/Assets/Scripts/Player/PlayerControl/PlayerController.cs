using UnityEngine;
using UnityEngine.InputSystem;
using Inventory.Model;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private AnimatorOverrideController withArmOverride;
    [SerializeField] private AnimatorOverrideController noArmOverride;
    [SerializeField] public SpriteRenderer playerSpriteRenderer;
    [SerializeField] private PlayerStamina _playerStamina;

    private PlayerStatus _playerStatus;
    private PlayerInteraction _playerInteraction;
    private PlayerMovement _playerMovement;
    private Animator _animator;
    private EquipWeapon _equipWeapon;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _runAction;
    private InputAction _dashAction;

    public PlayerState _currentState = PlayerState.Idle;
    public Vector2 _forwardVector;
    public bool IsFacingRight => transform.localScale.x < 0f;
    private bool _isInvincible = false;

    private float _damageCooldown = 0.05f;
    private float _lastDamageTime = -999f;

    private float _globalNextFireTime = 0f;
    public float GlobalNextFireTime
    {
        get => _globalNextFireTime;
        set => _globalNextFireTime = value;
    }

    private void Awake()
    {
        _playerStatus = GetComponent<PlayerStatus>();
        _playerInteraction = GetComponent<PlayerInteraction>();
        _playerMovement = GetComponent<PlayerMovement>();
        _equipWeapon = GetComponent<EquipWeapon>();
        _animator = GetComponent<Animator>();

        _playerInput = new PlayerInput();

        _moveAction = _playerInput.Player.Move;
        _runAction = _playerInput.Player.Run;
        _dashAction = _playerInput.Player.Dash;

    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _moveAction.Enable();
        _runAction.Enable();
        _dashAction.Enable();

        _playerInput.Player.Reload.performed += PerformReload;

        Managers.Event.Subscribe("OnPlayerDead", OnPlayerDead);
        Managers.Event.Subscribe("OnCinematicStart", OnEnterCinematic);
        Managers.Event.Subscribe("OnCinematicEnd", OnExitCinematic);
    }


    private void OnDisable()
    {
        _moveAction.Disable();
        _runAction.Disable();
        _dashAction.Disable();

        _playerInput.Player.Reload.performed -= PerformReload;

        Managers.Event.Unsubscribe("OnPlayerDead", OnPlayerDead);
        Managers.Event.Unsubscribe("OnCinematicStart", OnEnterCinematic);
        Managers.Event.Unsubscribe("OnCinematicEnd", OnExitCinematic);
    }


    private bool _prevHasWeapon = false;

    private void Update()
    {
        if (_currentState == PlayerState.Dead || _currentState == PlayerState.Cinematic)
            return;

        Transform socket = _equipWeapon.WeaponSocket;
        if (socket == null)
        {
            Debug.LogWarning("소켓이 비어 있습니다.");
            return;
        }

        bool hasWeapon = socket.childCount > 0;

        if (hasWeapon != _prevHasWeapon)
        {
            _animator.runtimeAnimatorController = hasWeapon ? noArmOverride : withArmOverride;
            _prevHasWeapon = hasWeapon;
        }


        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        bool isMoving = moveInput != Vector2.zero;
        bool wantsRun = _runAction.IsPressed();
        bool isAlreadyRunning = _playerStamina.isRunning;
        bool canRun = wantsRun && _playerStamina.CanRun(isAlreadyRunning);

        if (isMoving)
            _forwardVector = moveInput;

        _playerMovement.Move(moveInput, canRun);

        if (canRun && isMoving)
        {
            _playerStamina.UseRunStamina();
        }

        _playerStamina.isRunning = canRun;

        if (_dashAction.triggered)
        {
            if (_playerStamina.CanDash())
            {
                _playerStamina.UseDashStamina();
                _playerMovement.TryDash(_forwardVector);
            }
            else
            {
                Debug.Log("스태미나 부족 - 대쉬 불가");
            }
        }

        bool isDashing = _playerMovement.IsDashing;

        _animator.SetBool("isWalking", isMoving && !canRun);
        _animator.SetBool("isRunning", isMoving && canRun);
        _animator.SetBool("isDashing", isDashing);

        _currentState = isMoving ? PlayerState.Move : PlayerState.Idle;

        if (EventSystem.current.IsPointerOverGameObject() == false && Mouse.current.leftButton.wasPressedThisFrame)
        {
            _equipWeapon?.StartAttack(this);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _equipWeapon?.StopAttack();
        }
    }

    private void OnPlayerDead(object obj)
    {
        if (_currentState == PlayerState.Dead) return;
        _currentState = PlayerState.Dead;

        _animator.SetBool("isDead", true);
        _playerMovement.StopImmediately();
        enabled = false;
    }

    public void TakeDamage(GameObject attacker, float damageValue)
    {
        if (_currentState == PlayerState.Dead || _currentState == PlayerState.Cinematic || _isInvincible)
            return;

        if (Time.time < _lastDamageTime + _damageCooldown)
            return;

        _lastDamageTime = Time.time;
        _playerStatus.OnDamaged(attacker, damageValue);
    }

    public void TakeHeal(float healValue)
    {
        _playerStatus.ApplyHealChange(healValue);
    }
    public bool IsDead() => _currentState == PlayerState.Dead;

    private void OnEnterCinematic(object obj)
    {
        if (_currentState == PlayerState.Dead) return;

        _currentState = PlayerState.Cinematic;
        _playerMovement.StopImmediately();

        _equipWeapon?.StopAttack();
    }

    private void OnExitCinematic(object obj)
    {
        if (_currentState == PlayerState.Cinematic)
        {
            _currentState = PlayerState.Idle;
            StartCoroutine(InvincibilityCoroutine(0.5f));
        }
    }
    private IEnumerator InvincibilityCoroutine(float duration)
    {
        _isInvincible = true;
        yield return new WaitForSeconds(duration);
        _isInvincible = false;
    }

    public void Move(Vector2 moveInput, bool isRunning)
    {
        if (moveInput.x != 0)
            playerSpriteRenderer.flipX = moveInput.x < 0;
    }
    private void PerformReload(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Reload");
        _equipWeapon?.Reload();
    }

}