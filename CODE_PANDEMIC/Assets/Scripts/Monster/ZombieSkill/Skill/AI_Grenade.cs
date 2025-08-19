using System.Collections;
using UnityEngine;

public class AI_Grenade : MonoBehaviour
{
    private float _damage;
    private float _radius;
    private float _delay;
    private LayerMask _targetLayer;
    private GameObject _explosionEffect;
    private bool _hasExploded = false;

    public void Initialize(float damage, float radius, float delay, LayerMask targetLayer , GameObject explosionEffect)
    {
        _damage = damage;
        _radius = radius;
        _delay = delay;
        _targetLayer = targetLayer;
        _explosionEffect = explosionEffect;
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(_delay);
        Explode();
    }

    private void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius, _targetLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<PlayerController>(out var player))
            {
                player.TakeDamage(gameObject, _damage);
            }
        }

        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _targetLayer) != 0)
        {
            Explode();
        }
        else
        {
            // 다른 오브젝트 충돌에도 터뜨리려면
            // Explode();
        }
    }
}
