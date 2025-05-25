using UnityEngine;
using UnityEngine.InputSystem;
using Inventory.Model;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private EquipWeapon _equipWeapon;
    [SerializeField] private Transform _weaponHolder;

    private PlayerStatus _playerStatus;
    private PlayerInteraction _playerInteraction;
    private PlayerMovement _playerMovement;
    private Animator _animator;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _runAction;
    private InputAction _dashAction;

    public PlayerState _currentState = PlayerState.Idle;
    public Vector2 _forwardVector;
    public bool IsFacingRight => transform.localScale.x < 0f;

    [SerializeField] private AnimatorOverrideController withArmOverride;
    [SerializeField] private AnimatorOverrideController noArmOverride;



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
        _moveAction.Enable();
        _runAction.Enable();
        _dashAction.Enable();

        Managers.Event.Subscribe("OnPlayerDead", OnPlayerDead);
        Managers.Event.Subscribe("OnCinematicStart", OnEnterCinematic);
        Managers.Event.Subscribe("OnCinematicEnd", OnExitCinematic);
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _runAction.Disable();
        _dashAction.Disable();

        Managers.Event.Unsubscribe("OnPlayerDead", OnPlayerDead);
        Managers.Event.Unsubscribe("OnCinematicStart", OnEnterCinematic);
        Managers.Event.Unsubscribe("OnCinematicEnd", OnExitCinematic);
    }

    private bool _prevHasWeapon = false;

    private void Update()
    {
        if (_currentState == PlayerState.Dead) return;

        Transform socket = _equipWeapon.WeaponSocket;
        if (socket == null)
        {
            Debug.LogWarning("소켓이 비어 있습니다.");
            return;
        }

        bool hasWeapon = socket.childCount > 0;

        if (hasWeapon != _prevHasWeapon)
        {
            _animator.runtimeAnimatorController = hasWeapon ? withArmOverride : noArmOverride;
            _prevHasWeapon = hasWeapon;
        }

        // 이동 입력
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        bool isMoving = moveInput != Vector2.zero;
        bool isRunning = _runAction.IsPressed();

        if (isMoving)
            _forwardVector = moveInput;

        _playerMovement.Move(moveInput, isRunning);

        bool isDashing = _playerMovement.IsDashing;

        // 애니메이션 처리
        _animator.SetBool("isWalking", isMoving && !isRunning);
        _animator.SetBool("isRunning", isMoving && isRunning);
        _animator.SetBool("isDashing", isDashing);

        _currentState = isMoving ? PlayerState.Move : PlayerState.Idle;

        if (_dashAction.triggered)
        {
            _playerMovement.TryDash(_forwardVector);
        }

        if (Mouse.current.leftButton.isPressed)
        {
            _equipWeapon?.StartAttack(this);
        }
        else
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
        _playerStatus.OnDamaged(attacker, damageValue);
    }

    public void TakeHeal(float healValue)
    {
        _playerStatus.OnHealed(healValue);
    }
    public bool IsDead() => _currentState == PlayerState.Dead;

    private void OnEnterCinematic(object obj)
    {
        if (_currentState == PlayerState.Dead) return;

        _currentState = PlayerState.BossCinematic;
        _playerMovement.StopImmediately();

     
    }

    private void OnExitCinematic(object obj)
    {
        if (_currentState == PlayerState.BossCinematic)
        {
            _currentState = PlayerState.Idle;
            
        }
    }

}