using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �⺻ �̵� �ӵ�
    public float sprintSpeed = 8f; // �޸���(Shift) �ӵ�

    private Vector2 movement;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input Manager���� �̵� �Է°� ��������
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        movement = new Vector2(moveX, moveY).normalized;

        // Shift ������ ������Ʈ, �ƴϸ� �⺻ �ӵ�
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
        // Rigidbody2D�� �̿��� �̵� ó��
        rb.velocity = movement * moveSpeed;
    }
}
