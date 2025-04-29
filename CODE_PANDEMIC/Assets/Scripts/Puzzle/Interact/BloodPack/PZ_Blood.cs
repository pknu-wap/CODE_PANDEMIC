using UnityEngine;
using System.Collections;

public class PZ_Blood : MonoBehaviour
{
    private PlayerController _playerController;
    private Rigidbody2D _rigidBody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _playerController = collision.GetComponent<PlayerController>();
            _rigidBody = collision.GetComponent<Rigidbody2D>();

            StartCoroutine(PlayerSliding(_playerController._forwardVector));
        }
    }

    private IEnumerator PlayerSliding(Vector2 fv)
    {
        float currentTime = 0f;
        float moveDuration = 0.2f;

        Vector2 forwardVector = fv;
        float speed = 4f;

        while (currentTime < moveDuration)
        {
            currentTime += Time.fixedDeltaTime;

            _rigidBody.position += forwardVector.normalized * speed * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
    }
}