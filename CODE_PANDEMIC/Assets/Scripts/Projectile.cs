using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;

    public void Launch(Vector3 direction)
    {
        this.direction = direction.normalized;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
     
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}

