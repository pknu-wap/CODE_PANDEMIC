using UnityEngine;
using System.Collections;

public class AI_ContaminatedArea : MonoBehaviour
{
    private float _duration = 10f;
    private float _damagePerSecond = 5f;
    private float _radius = 1f;
    private float _interval = 1f;
 // TODO : 원장 좀비 장판 크기 키우기 
    private void Start()
    {
        StartCoroutine(DamageOverTime());
    }

    private IEnumerator DamageOverTime()
{
    float elapsed = 0f;

    while (elapsed < _duration)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _radius, LayerMask.GetMask("Player"));
        foreach (var target in targets)
        {
            if (target.TryGetComponent<PlayerController>(out var player))
            {
                player.TakeDamage(gameObject, _damagePerSecond);
            }
        }

        yield return new WaitForSeconds(_interval);
        elapsed += _interval;
    }

    StartCoroutine(FadeOut());
}

    private IEnumerator FadeOut()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Destroy(gameObject);
            yield break;
        }

        float fadeDuration = 1f;
        float fadeElapsed = 0f;
        Color originalColor = sr.color;

        while (fadeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, fadeElapsed / fadeDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            fadeElapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
