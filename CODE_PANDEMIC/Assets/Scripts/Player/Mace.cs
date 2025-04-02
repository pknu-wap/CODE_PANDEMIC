using System.Collections;
using UnityEngine;


public class Mace : MonoBehaviour
{
    private float speed = 12f;
    private float range = 10f;

    private Vector2 direction;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mousePos - (Vector2)transform.position).normalized;
            StartCoroutine(ThrowMace());
        }
    }

    IEnumerator ThrowMace()
    {
        float distanceTravelled = 0f;
        while (distanceTravelled < range)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            distanceTravelled += speed * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
