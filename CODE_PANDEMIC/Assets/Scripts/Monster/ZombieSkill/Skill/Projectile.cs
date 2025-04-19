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

        if (collision.CompareTag("Player") || collision.CompareTag("Obstacle"))
        {
            Vector2 spawnPos;

            if (collision.CompareTag("Player"))
            {
                // 플레이어의 현재 위치
                spawnPos = collision.transform.position;
            }
            else
            {
                // 장애물에 부딪혔을 때는 발사체 위치
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
