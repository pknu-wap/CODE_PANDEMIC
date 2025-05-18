using UnityEngine;
using System.Collections;

public class PZ_TrafficLight : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _damageBox;

    [SerializeField] private Sprite _green;
    [SerializeField] private Sprite _yellow;
    [SerializeField] private Sprite _red;

    private bool _isDamageTime = false;
    private float _damageValue = 10;

    private void Start()
    {
        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        while (true)
        {
            _isDamageTime = false;
            _spriteRenderer.sprite = _green;
            yield return new WaitForSeconds(10);

            _spriteRenderer.sprite = _yellow;
            yield return new WaitForSeconds(2);

            _isDamageTime = true;
            _spriteRenderer.sprite = _red;
            yield return new WaitForSeconds(5);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!_isDamageTime || !collision.GetComponentInParent<PlayerController>())
        {
            return;
        }

        if (collision.GetComponentInParent<PlayerController>()._forwardVector != Vector2.zero)
        {
            collision.GetComponentInParent<PlayerController>().TakeDamage(null, _damageValue * Time.deltaTime);
        }
    }
}