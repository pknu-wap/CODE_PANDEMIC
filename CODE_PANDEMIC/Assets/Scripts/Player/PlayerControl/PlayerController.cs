using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private EquipWeapon _equipWeapon;

    private PlayerMovement _playerMovement;
    private PlayerStatus _playerStatus;
    private PlayerInteraction _playerInteraction;
    private Animator _animator;

    private PlayerInput _playerInput;
    private InputAction _moveAction;

    public PlayerState _currentState = PlayerState.Idle;
    public Vector2 _forwardVector;

    [SerializeField] public Transform _weaponHolder;

    #region Base

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerStatus = GetComponent<PlayerStatus>();
        _playerInteraction = GetComponent<PlayerInteraction>();
        _equipWeapon = GetComponent<EquipWeapon>();
        _animator = GetComponent<Animator>();

        _playerInput = new PlayerInput();
        _moveAction = _playerInput.Player.Move;
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        Managers.Event.Subscribe("OnPlayerDead", OnPlayerDead);
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        Managers.Event.Unsubscribe("OnPlayerDead", OnPlayerDead);
    }

    #endregion

    #region Status

    public void TakeDamage(GameObject attacker, float damageValue)
    {
        _playerStatus.OnDamaged(attacker, damageValue);
    }

    public void TakeHeal(float healValue)
    {
        _playerStatus.OnHealed(healValue);
    }

    #endregion

    #region Die

    private void OnPlayerDead(object obj)
    {
        if (_currentState == PlayerState.Dead) return;

        _currentState = PlayerState.Dead;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        _animator.SetBool("isDead", true);

        enabled = false;
    }

    public bool IsDead()
    {
        return _currentState == PlayerState.Dead;
    }

    #endregion

    #region Movement Animation (Idle / Move)

    private void Update()
    {
        if (_currentState == PlayerState.Dead) return;

        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        bool isMoving = moveInput != Vector2.zero;

        bool isRunning = Keyboard.current.leftShiftKey.isPressed;
        bool isDashing = Keyboard.current.spaceKey.wasPressedThisFrame;

        _animator.SetBool("isWalking", isMoving && !isRunning);
        _animator.SetBool("isRunning", isMoving && isRunning);
        _animator.SetBool("isDashing", isDashing);

    }


    #endregion
}
