using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float MaxDistance = 7f;
    public GameObject contaminatedAreaPrefab;
    private Vector2 _startPos;
    private Rigidbody2D _rb;
    private bool _hasTriggered = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _startPos = transform.position;
        _hasTriggered = false;
    }

    private void FixedUpdate()
    {
        if (_hasTriggered) return;

        float dist = Vector2.Distance(_startPos, transform.position);
        if (dist >= MaxDistance)
        {
            TriggerContamination(transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasTriggered) return;

        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0 || 
            ((1 << collision.gameObject.layer) & LayerMask.GetMask("Obstacle")) != 0)
        {
            Vector2 spawnPos;

            if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
            {
                spawnPos = collision.transform.position;
            }
            else
            {
                spawnPos = transform.position;
            }

            TriggerContamination(spawnPos);
        }
    }

    private void TriggerContamination(Vector2 spawnPosition)
    {
        _hasTriggered = true;
        Instantiate(contaminatedAreaPrefab, spawnPosition, Quaternion.identity);
        Destroy(gameObject);
    }
}
