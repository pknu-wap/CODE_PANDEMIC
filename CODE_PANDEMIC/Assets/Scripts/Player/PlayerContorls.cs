using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject revolverPrefab;

    private WeaponBase equippedWeapon;

    PlayerControls action;
    InputAction moveAction;

    private void Awake()
    {
        action = new PlayerControls();
        moveAction = action.Player.Move;
    }

    private void Start()
    {
        if (revolverPrefab != null)
        {
            GameObject weaponObj = Instantiate(revolverPrefab, transform);
            weaponObj.transform.localPosition = Vector3.zero;
            equippedWeapon = weaponObj.GetComponent<WeaponBase>();
        }
    }

    private void OnEnable()
    {
        moveAction.Enable();
        moveAction.started += Started;
        moveAction.performed += Performed;
        moveAction.canceled += Canceled;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        moveAction.started -= Started;
        moveAction.performed -= Performed;
        moveAction.canceled -= Canceled;
    }

    private void FixedUpdate()
    {
        Vector2 keyboard_vector = moveAction.ReadValue<Vector2>();
        MOVE(keyboard_vector.x, keyboard_vector.y);
    }

    private void Update()
    {
        if (equippedWeapon != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            equippedWeapon.Attack();
        }
    }

    void Started(InputAction.CallbackContext context)
    {
        Debug.Log("started!");
    }

    void Performed(InputAction.CallbackContext context)
    {
        Debug.Log("performed!");
    }

    void Canceled(InputAction.CallbackContext context)
    {
        Debug.Log("canceled!");
    }

    void MOVE(float _x, float _y)
    {
        float moveSpeed = 5f;
        Vector2 moveDir = new Vector2(_x, _y);
        this.transform.position += (Vector3)(moveDir * moveSpeed * Time.fixedDeltaTime);
    }
}
