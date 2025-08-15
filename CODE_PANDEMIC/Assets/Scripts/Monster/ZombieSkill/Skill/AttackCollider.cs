using UnityEngine;
using System.Collections;

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
        StartCoroutine(DeactivateRoutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _targetLayer) != 0)
        {
            if (other.TryGetComponent<PlayerController>(out var player))
            {
                player.TakeDamage(gameObject, _damage);
            }
        }
    }

    private IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(_lifetime);
        gameObject.SetActive(false);
    }
}
