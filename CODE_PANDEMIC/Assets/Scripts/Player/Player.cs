using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float playerSpeed = 1f;
    private float playerRunMultiplier = 2f;
    private Vector2 move;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private WeaponBase equippedWeapon;
    [SerializeField] private GameObject macePrefab;
    [SerializeField] private GameObject gunPrefab;

    private float fireTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        if (macePrefab != null)
        {
            EquipWeapon(Instantiate(macePrefab, transform).GetComponent<WeaponBase>());
        }
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        move = new Vector2(horizontal, vertical).normalized;

        if (horizontal != 0)
            spriteRenderer.flipX = horizontal < 0;

        if (equippedWeapon != null && Input.GetMouseButtonDown(0) && fireTimer <= 0f)
        {
            equippedWeapon.Attack();
            fireTimer = equippedWeapon.fireRate;
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwapWeapon(macePrefab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwapWeapon(gunPrefab);
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? playerSpeed * playerRunMultiplier : playerSpeed;
        rb.velocity = move * currentSpeed;
    }

    private void SwapWeapon(GameObject weaponPrefab)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }

        WeaponBase newWeapon = Instantiate(weaponPrefab, transform).GetComponent<WeaponBase>();
        EquipWeapon(newWeapon);
    }

    public void EquipWeapon(WeaponBase newWeapon)
    {
        equippedWeapon = newWeapon;
        equippedWeapon.transform.parent = transform;
        equippedWeapon.transform.localPosition = Vector3.zero;
    }
}
