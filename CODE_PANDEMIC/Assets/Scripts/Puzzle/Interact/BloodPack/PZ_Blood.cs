using UnityEngine;
using System.Collections;

public class PZ_Blood : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(CharacterSliding(collision.GetComponent<Rigidbody2D>(), collision.GetComponent<PlayerController>()._forwardVector));
        }
    }

    private IEnumerator CharacterSliding(Rigidbody2D rigidbody2D, Vector2 fv)
    {
        float currentTime = 0f;
        float moveDuration = 0.2f;

        Vector2 forwardVector = fv;
        float speed = 4f;

        while (currentTime < moveDuration)
        {
            currentTime += Time.fixedDeltaTime;

            rigidbody2D.position += forwardVector.normalized * speed * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
    }
}