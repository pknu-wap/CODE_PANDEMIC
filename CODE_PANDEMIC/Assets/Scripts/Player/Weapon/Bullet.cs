using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        CancelInvoke();
        Invoke("ReturnToPool", lifeTime);
        Debug.Log($"�Ѿ� ������ at {transform.position}");
    }

    public void Fire(Vector2 direction)
    {
        Debug.Log("�Ѿ� �߻�� ����: " + direction);
        rb.velocity = direction.normalized * speed;
    }



    private void ReturnToPool()
    {
        BulletPool.Instance.ReturnBullet(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return; // �ʿ� �� ���� �߰�
        ReturnToPool();
    }
}
