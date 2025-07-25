using UnityEngine;

public class PZ_BloodPack : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;

    private float _speed = 20f; // 던지는 속도
    private float _distance = 1.0f; // 들고 있는 거리
    private int _healValue = 10;

    private bool _isThrowing = false;

    private PlayerController _playerController;

    [SerializeField] private GameObject _blood;

    public void Setting(PlayerController playerController)
    {
        _playerController = playerController;
    }

    private void Update()
    {
        if (_isThrowing)
        {
            return;
        }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PolygonCollider2D>())
        {
            return;
        }

        if (_isThrowing && collision.GetComponent<AI_Base>()) // 던져서 터트리기
        {
            Instantiate(_blood, transform.position, transform.rotation);

            collision.GetComponent<AI_Base>().TakeDamage(-_healValue); // 피 채워 주기

            Destroy(gameObject);
        }

        else if (_isThrowing && !collision.GetComponent<AI_Base>())
        {
            Instantiate(_blood, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}