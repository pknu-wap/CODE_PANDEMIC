using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SoliderBullet : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;
    private float _damage;
    private float _maxDistance;
    private LayerMask _targetLayer;

    private Vector2 _startPos;

    public void Initialize(Vector2 direction, float speed, float damage, float range, LayerMask targetLayer)
    {
        _direction = direction.normalized;
        _speed = speed;
        _damage = damage;
        _maxDistance = range;
        _targetLayer = targetLayer;
        _startPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);

        if (Vector2.Distance(_startPos, transform.position) >= _maxDistance)
        {
            Destroy(gameObject);
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, 0.1f, _targetLayer);
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<PlayerController>(out var player))
            {
                player.TakeDamage(gameObject, _damage);
            }
            Destroy(gameObject);
        }
    }
}
