using System.Collections;
using UnityEngine;

public class ThunderStrike : MonoBehaviour
{
    private int _damage;
    private float _delay;
    private float _radius;
    private LayerMask _targetLayer;

    public void Initialize(int damage, float delay, float radius, LayerMask targetLayer)
    {
        _damage = damage;
        _delay = delay;
        _radius = radius;
        _targetLayer = targetLayer;

        StartCoroutine(StrikeRoutine());
    }

    private IEnumerator StrikeRoutine()
    {
        yield return new WaitForSeconds(_delay);

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _radius, _targetLayer);
        foreach (Collider2D target in targets)
        {
            if (target.TryGetComponent<PlayerController>(out var damageable))
            {
                damageable.TakeDamage(gameObject, _damage);
            }
        }

        Destroy(gameObject);
    }
}
