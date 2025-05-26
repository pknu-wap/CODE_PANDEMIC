using UnityEngine;

public class PZ_FireExtinguisher : PZ_Interact_NonSpawn
{
    [SerializeField] private Rigidbody2D _rigidbody;

    private float _speed = 20f; // 던지는 속도
    private float _distance = 1.0f; // 들고 있는 거리
    private int _damage = 100;

    private bool _isThrowing = false;
    [SerializeField] private GameObject _explosionEffect;

    private PlayerController _playerController;

    private void Update()
    {
        if (!_isInteracted)
        {
            return;
        }

        // 소화기 들기
        if (!_isThrowing)
        {
            Vector3 _temp = _playerController.transform.position;
            _temp.x += _playerController._forwardVector.normalized.x * _distance;
            _temp.y += _playerController._forwardVector.normalized.y * _distance;

            transform.position = _temp;

            // 던져버리기
            if (Input.GetKeyDown(KeyCode.F))
            {
                _isThrowing = true;
                _rigidbody.isKinematic = false;
                _rigidbody.AddForce(_playerController._forwardVector.normalized * _speed, ForceMode2D.Impulse);
            }
        }
    }

    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);
        Managers.Game.AddInteractCount(Define.InteractType.Extinguisher);
        _playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PolygonCollider2D>())
        {
            return;
        }

        if (_isInteracted && _isThrowing) // 던져서 터트리기
        {
            AI_Base monster = collision.GetComponent<AI_Base>();

            if (monster)
            {
                monster.TakeDamage(_damage);
            }

            Instantiate(_explosionEffect, transform.position, transform.rotation);

            Destroy(gameObject);
        }

        if (!_isInteracted && collision.gameObject.GetComponent<Bullet>()) // 총알로 터트리기
        {
            Instantiate(_explosionEffect, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}