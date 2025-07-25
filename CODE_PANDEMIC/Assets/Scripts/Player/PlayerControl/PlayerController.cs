using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using Inventory.Model;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerAnimationHandler))]
[RequireComponent(typeof(PlayerCombatHandler))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStatus))]
[RequireComponent(typeof(PlayerStamina))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    public PlayerState _currentState = PlayerState.Idle;
    public Vector2 _forwardVector;
    public float GlobalNextFireTime { get; set; } = 0f;
    public bool IsFacingRight => transform.localScale.x < 0f;

    private PlayerInputHandler _inputHandler;
    private PlayerAnimationHandler _animationHandler;
    private PlayerCombatHandler _combatHandler;
    private PlayerMovement _playerMovement;
    private PlayerStatus _playerStatus;
    private PlayerStamina _playerStamina;

    private bool _isInvincible = false;
    private float _lastDamageTime = -999f;
    private const float _damageCooldown = 0.05f;

    private void Awake()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        _animationHandler = GetComponent<PlayerAnimationHandler>();
        _combatHandler = GetComponent<PlayerCombatHandler>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerStatus = GetComponent<PlayerStatus>();
        _playerStamina = GetComponent<PlayerStamina>();

        _combatHandler.Initialize(this);
    }

    private void OnEnable()
    {
        _inputHandler.EnableInput();
        _combatHandler.SubscribeInput();

        Managers.Event.Subscribe("OnPlayerDead", OnPlayerDead);
        Managers.Event.Subscribe("OnCinematicStart", OnEnterCinematic);
        Managers.Event.Subscribe("OnCinematicEnd", OnExitCinematic);
    }

    private void OnDisable()
    {
        _inputHandler.DisableInput();
        _combatHandler.UnsubscribeInput();

        Managers.Event.Unsubscribe("OnPlayerDead", OnPlayerDead);
        Managers.Event.Unsubscribe("OnCinematicStart", OnEnterCinematic);
        Managers.Event.Unsubscribe("OnCinematicEnd", OnExitCinematic);
    }

    private void Update()
    {
        if (_currentState == PlayerState.Dead || _currentState == PlayerState.Cinematic)
            return;

        Vector2 moveInput = _inputHandler.GetMoveInput();
        bool isMoving = moveInput != Vector2.zero;
        bool wantsRun = _inputHandler.IsRunPressed();
        bool isAlreadyRunning = _playerStamina.isRunning;
        bool canRun = wantsRun && _playerStamina.CanRun(isAlreadyRunning);

        if (isMoving)
            _forwardVector = moveInput;

        _playerMovement.Move(moveInput, canRun);
        _playerStamina.isRunning = canRun;

        if (canRun && isMoving)
            _playerStamina.StartRunning();

        if (_inputHandler.IsDashTriggered())
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

        _animationHandler.UpdateAnimationStates(isMoving, canRun, _playerMovement.IsDashing);
        _currentState = isMoving ? PlayerState.Move : PlayerState.Idle;

        _combatHandler.HandleAttackInput();
    }

    private void OnPlayerDead(object obj)
    {
        if (_currentState == PlayerState.Dead) return;
        _currentState = PlayerState.Dead;

        _animationHandler.SetDeadAnimation();
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
        _combatHandler.StopAttack();
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
}