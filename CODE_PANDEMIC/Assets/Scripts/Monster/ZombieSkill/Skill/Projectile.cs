using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float MaxDistance = 7f;
    public GameObject contaminatedAreaPrefab;
    private Vector2 _startPos;
    private Rigidbody2D _rb;
    private bool _hasTriggered = false;
    private PlayerController player;
    private AI_Controller _owner;
    private float _safeTime = 0.05f; // 발사 직후 0.05초 동안 충돌 무시
    private float _spawnTime;

    public void SetOwner(AI_Controller owner)
    {
        _owner = owner;
    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
    }

    private void OnEnable()
    {
        _startPos = transform.position;
        _hasTriggered = false;
        _spawnTime = Time.time;
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
        if (_hasTriggered || Time.time - _spawnTime < _safeTime) return;

        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0 || 
            ((1 << collision.gameObject.layer) & LayerMask.GetMask("Wall")) != 0)
        {
            Vector2 spawnPos;

            if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
            {
                player.TakeDamage(gameObject, _owner.AiDamage);
                spawnPos = transform.position;
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
        Transform parent = _owner?.transform.parent;
        GameObject area = Instantiate(contaminatedAreaPrefab, spawnPosition, Quaternion.identity , parent);
        float scaleFactor = _owner.transform.localScale.x * 0.5f;
        area.transform.localScale = Vector3.one * scaleFactor;
        Destroy(gameObject);
    }
}
