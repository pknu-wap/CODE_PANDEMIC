using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private PlayerInput _action;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction dashAction;

    private Vector2 moveInput;
    private WeaponBase equippedWeapon;

    private Rigidbody2D rb;
    private bool isDashing = false;
    private float lastDashTime = -999f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _action = new PlayerInput();

        moveAction = _action.Player.Move;
        runAction = _action.Player.Run;
        dashAction = _action.Player.Dash;
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
        if (isDashing) return;

        moveInput = moveAction.ReadValue<Vector2>();
        float speed = runAction.IsPressed() ? runSpeed : walkSpeed;

        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);

        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }

    private void Update()
    {
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
    }

    public void EquipWeapon(WeaponBase newWeapon)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }

        equippedWeapon = newWeapon;
        equippedWeapon.transform.SetParent(transform);
        equippedWeapon.transform.localPosition = Vector3.zero;
    }
}
