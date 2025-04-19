using UnityEngine;
using System.Collections;

public class AI_ContaminatedArea : MonoBehaviour
{
    private float _duration = 10f;
    private float _damagePerSecond = 5f;
    private float _radius = 1f;
    private float _interval = 1f;

    private void Start()
    {
        StartCoroutine(DamageOverTime());
    }

    private IEnumerator DamageOverTime()
    {
        float elapsed = 0f;
        while (elapsed < _duration)
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _radius, LayerMask.GetMask("Player"));
            foreach (var target in targets)
            {
                if (target.TryGetComponent<PlayerController>(out var player) && !player.IsDead())
                {
                    // player.TakeDamage(_damagePerSecond);
                    Debug.Log($"{_damagePerSecond} 데미지");
                }
            }

            yield return new WaitForSeconds(_interval);
            elapsed += _interval;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
