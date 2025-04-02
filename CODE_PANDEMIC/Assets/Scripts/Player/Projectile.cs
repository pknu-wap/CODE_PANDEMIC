using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 10f;
    private float lifetime = 5f;
    private float attackRange = 0.8f;

    private Vector2 direction;

    void Start()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePos - (Vector2)transform.position).normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Vector2.Dot(direction, (collision.transform.position - transform.position).normalized) > attackRange)
        {
            Destroy(gameObject);
        }
    }
}
