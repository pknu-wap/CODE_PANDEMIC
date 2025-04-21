using UnityEngine;

public class PZ_FireExtinguisher : MonoBehaviour, IInteractable
{
    [SerializeField] private Rigidbody2D _rigidbody;
    private float _speed = 20f; // 던지는 속도
    private float _distance = 1.5f; // 들고 있는 거리

    private bool _isInteracted = false;
    private bool _isThrowing = false;
    [SerializeField] private GameObject _explosionEffect;

    private PlayerController _playerController;

    // 하이라이트 기능
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _highlightMaterial;

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

    public void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        _isInteracted = true;

        _playerController = player.GetComponent<PlayerController>();
    }

    public bool IsInteractable()
    {
        return _isInteracted;
    }

    public void OnHighLight()
    {
        _spriteRenderer.material = _highlightMaterial;
    }

    public void OffHighLight()
    {
        _spriteRenderer.material = _defaultMaterial;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isInteracted && _isThrowing) // 던져서 터트리기
        {
            Instantiate(_explosionEffect, transform.position, transform.rotation);

            Destroy(gameObject);
        }

        if (!_isInteracted && collision.GetComponent<Bullet>()) // 총알로 터트리기
        {
            Instantiate(_explosionEffect, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}