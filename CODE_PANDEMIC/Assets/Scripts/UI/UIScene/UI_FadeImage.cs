
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeImage : UI_Base
{
    enum Images
    {
        FadeImage,
    }

    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField]
    private Image _fadeImage;
    private bool _isFading = false;

    public override bool Init()
    {

        if (!base.Init()) return false;

        BindImage(typeof(Images));
        _fadeImage = GetImage((int)Images.FadeImage);

        if (_fadeImage == null)
        {
            Debug.LogError("UI_FadeImage: FadeImage Image is NULL.");
        }

        return _fadeImage != null;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, Action onComplete)
    {
        _isFading = true;

        float elapsed = 0f;
        Color original = _fadeImage.color;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / _fadeDuration);
            _fadeImage.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }

        _fadeImage.color = new Color(original.r, original.g, original.b, endAlpha);
        _isFading = false;
        onComplete?.Invoke();
    }

    public void FadeIn(Action onComplete = null)
    {
        if (_isFading) return;

        StartCoroutine(Fade(1f, 0f, () =>
        {
            onComplete?.Invoke();
            this.gameObject.SetActive(false);
        }));

    }

    public void FadeOut(Action onComplete = null)
    {
        if (_isFading) return;

        StartCoroutine(Fade(0f, 1f, onComplete));
    }
}
