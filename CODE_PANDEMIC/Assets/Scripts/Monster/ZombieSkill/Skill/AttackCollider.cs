using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private int damage = 10;
    private float lifeTime = 0.3f;
    private LayerMask targetLayer;

    private bool _hasDealtDamage = false;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasDealtDamage) return;
        targetLayer = LayerMask.GetMask("Player");

        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            PlayerStatus player = collision.GetComponent<PlayerStatus>();
                player.OnDamaged(gameObject, damage);
                _hasDealtDamage = true;
        }
    }
}
