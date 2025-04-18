using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    private float _damage;
    private LayerMask _targetMask;

    public void Initialize(Vector2 velocity, float damage, LayerMask targetMask)
    {
        _damage = damage;
        _targetMask = targetMask;
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _targetMask) != 0)
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // player.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {

        GetComponent<Collider2D>().isTrigger = true;
    }
}
