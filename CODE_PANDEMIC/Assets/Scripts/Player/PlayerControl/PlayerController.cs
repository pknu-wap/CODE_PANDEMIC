using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Idle,
    Move,
    Dead
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashCooldown = 0.5f;

    [Header("Weapon Holder")]
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Vector3 weaponLocalPosition = new Vector3(-0.5f, 0f, 0f);

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private PlayerInput action;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction dashAction;

    private Vector2 moveInput;
    private WeaponBase equippedWeapon;

    private Rigidbody2D rb;
    private bool isDashing = false;
    private float lastDashTime = -999f;

    private PlayerState currentState = PlayerState.Idle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        action = new PlayerInput();

        moveAction = action.Player.Move;
        runAction = action.Player.Run;
        dashAction = action.Player.Dash;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        runAction.Enable();
        dashAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        runAction.Disable();
        dashAction.Disable();
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.Dead || isDashing) return;

        moveInput = moveAction.ReadValue<Vector2>();
        float speed = runAction.IsPressed() ? runSpeed : walkSpeed;

        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);

        if (moveInput.x != 0)
        {

            spriteRenderer.flipX = moveInput.x > 0;


            if (weaponHolder != null)
            {
                Vector3 adjustedPosition = weaponLocalPosition;
                adjustedPosition.x *= spriteRenderer.flipX ? -1 : 1;
                weaponHolder.localPosition = adjustedPosition;
            }
        }

        currentState = moveInput != Vector2.zero ? PlayerState.Move : PlayerState.Idle;
    }

    private void Update()
    {
        if (currentState == PlayerState.Dead) return;

        if (equippedWeapon != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            equippedWeapon.Attack();
        }

        if (dashAction.triggered && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private System.Collections.IEnumerator DashCoroutine()
    {
        isDashing = true;
        lastDashTime = Time.time;

        gameObject.tag = "Invincible";

        Vector2 dashDirection = moveInput.normalized;
        float dashTime = 0f;

        while (dashTime < dashDuration)
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, dashDirection, dashSpeed * Time.fixedDeltaTime, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
                break;

            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            dashTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        isDashing = false;

        yield return new WaitForSeconds(0.3f);
        gameObject.tag = "Player";
    }

    public void EquipWeapon(WeaponBase newWeapon)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }

        equippedWeapon = newWeapon;
        equippedWeapon.transform.SetParent(weaponHolder);
        equippedWeapon.transform.localPosition = Vector3.zero;
        equippedWeapon.transform.localRotation = Quaternion.identity;
    }

    public void Die()
    {
        if (currentState == PlayerState.Dead) return;

        currentState = PlayerState.Dead;
        rb.velocity = Vector2.zero;
    }

    public bool IsDead()
    {
        return currentState == PlayerState.Dead;
    }
}
