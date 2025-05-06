using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed = 10f;
    private float _lifeTime = 3f;
    private int _damage = 0;
    PlayerController _onwer;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetInfo(int damage,PlayerController owner)
    {
        _damage = damage;
        _onwer= owner;
    }
    private void OnEnable()
    {
        CancelInvoke();
        Invoke("ReturnToPool", _lifeTime);
        Debug.Log($"ÃÑ¾Ë »ý¼ºµÊ at {transform.position}");
    }

    public void Fire(Vector2 direction)
    {
        Debug.Log("ÃÑ¾Ë ¹ß»çµÊ ¹æÇâ: " + direction);
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
        _onwer = null;
        ReturnToPool();
    }
}
