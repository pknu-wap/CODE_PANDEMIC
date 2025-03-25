using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 기본 이동 속도
    public float sprintSpeed = 8f; // 달리기(Shift) 속도

    private Vector2 movement;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input Manager에서 이동 입력값 가져오기
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        movement = new Vector2(moveX, moveY).normalized;

        // Shift 누르면 스프린트, 아니면 기본 속도
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = 5f;
        }
    }

    void FixedUpdate()
    {
        // Rigidbody2D를 이용한 이동 처리
        rb.velocity = movement * moveSpeed;
    }
}
