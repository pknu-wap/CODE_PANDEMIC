using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed = 10f;
    private float _lifeTime = 3f;
    private int _damage = 0;
    private PlayerController _owner;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetInfo(int damage)
    {
        _damage = damage;
         
    }
    private void OnEnable()
    {
        CancelInvoke();
        Invoke("ReturnToPool", _lifeTime);
    }

    public void Fire(Vector2 direction)
    {
        Debug.Log("발사되었습니다.: " + direction);
        rb.velocity = direction.normalized * _speed;
    }



    private void ReturnToPool()
    {
        BulletPool.Instance.ReturnBullet(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        AI_Base enemy= other.GetComponent<AI_Base>();
        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
        }
        _damage = 0;
        _owner = null;
        ReturnToPool();
    }
    public void SetOwner(PlayerController owner)
    {
        _owner = owner;
    }

    public PlayerController GetOwner()
    {
        return _owner;
    }
}
