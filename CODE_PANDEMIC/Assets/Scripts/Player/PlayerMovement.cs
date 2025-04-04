using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float playerSpeed = 1f;
    private float playerRunMultiplier = 2f;
    private Vector2 move;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleMovementInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        move = new Vector2(horizontal, vertical).normalized;

        if (horizontal != 0)
            spriteRenderer.flipX = horizontal < 0;
    }

    private void MovePlayer()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? playerSpeed * playerRunMultiplier : playerSpeed;
        rb.velocity = move * currentSpeed;
    }
}

