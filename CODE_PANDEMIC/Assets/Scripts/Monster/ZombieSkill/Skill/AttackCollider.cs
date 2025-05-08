using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private int _damage;
    private float _lifetime;
    private LayerMask _targetLayer;

    public void Initialize(int damage, float duration, LayerMask targetLayer)
    {
        _damage = damage;
        _lifetime = duration;
        _targetLayer = targetLayer;
        Invoke(nameof(DestroySelf), _lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _targetLayer) != 0)
        {
            if (other.TryGetComponent<PlayerStatus>(out var playerStatus))
            {
                playerStatus.OnDamaged(gameObject, _damage);
            }
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
