using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public float playerRunSpeed = 1.5f;
    private Vector3 move;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite rightSprite;
    public Sprite rightupSprite;
    public Sprite rightdownSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        move = new Vector3(horizontal, vertical, 0).normalized;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? playerSpeed * playerRunSpeed : playerSpeed;

        if (move.magnitude > 0)
        {
            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;

            if (angle > -22.5f && angle <= 22.5f)
            {
                spriteRenderer.sprite = rightSprite;
                spriteRenderer.flipX = false;
            }
            else if (angle > 22.5f && angle <= 67.5f)
            {
                spriteRenderer.sprite = rightupSprite;
                spriteRenderer.flipX = false;
            }
            else if (angle > 67.5f && angle <= 112.5f)
            {
                spriteRenderer.sprite = upSprite;
                spriteRenderer.flipX = false;
            }
            else if (angle > 112.5f && angle <= 157.5f)
            {
                spriteRenderer.sprite = rightupSprite;
                spriteRenderer.flipX = true;
            }
            else if (angle > 157.5f || angle <= -157.5f)
            {
                spriteRenderer.sprite = rightSprite;
                spriteRenderer.flipX = true;
            }
            else if (angle > -157.5f && angle <= -112.5f)
            {
                spriteRenderer.sprite = rightdownSprite;
                spriteRenderer.flipX = true;
            }
            else if (angle > -112.5f && angle <= -67.5f)
            {
                spriteRenderer.sprite = downSprite;
                spriteRenderer.flipX = true;
            }
            else if (angle > -67.5f && angle <= -22.5f)
            {
                spriteRenderer.sprite = rightdownSprite;
                spriteRenderer.flipX = false;
            }
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? playerSpeed * playerRunSpeed : playerSpeed;
        rb.velocity = move * currentSpeed;
    }
}
