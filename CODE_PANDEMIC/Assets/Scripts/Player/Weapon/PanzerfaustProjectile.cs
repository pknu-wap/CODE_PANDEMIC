using UnityEngine;

public class PanzerfaustProjectile : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject explosionEffect;

    private int damage;
    private float maxRange;
    private Vector3 startPosition;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public void SetRange(float range)
    {
        maxRange = range;
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= maxRange)
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    private void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (var enemyCol in enemies)
        {
            AI_Base enemy = enemyCol.GetComponent<AI_Base>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        Debug.Log("판처파우스트 폭발!!");
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
