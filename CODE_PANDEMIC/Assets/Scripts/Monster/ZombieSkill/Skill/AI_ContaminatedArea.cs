using UnityEngine;
using System.Collections;

public class AI_ContaminatedArea : MonoBehaviour
{
    private float _duration = 10f;
    private float _damagePerSecond = 5f;
    private Vector2 _damageBoxSize = new Vector2(2.5f, 1.2f);
    private float _interval = 1f;

    private void Start()
    {
        StartCoroutine(DamageOverTime());
    }

    private IEnumerator DamageOverTime()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            Collider2D[] targets = Physics2D.OverlapCapsuleAll(
                transform.position,
                _damageBoxSize,
                0f,
                LayerMask.GetMask("Player")
            );

            foreach (var target in targets)
            {
                if (target.TryGetComponent<AttackObjCollider>(out var player))
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