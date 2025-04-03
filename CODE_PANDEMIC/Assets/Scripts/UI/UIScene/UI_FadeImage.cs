using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeImage : UI_Base
{
    // 하나밖에 없지만, 유지보수를 위해 Enum 사용
    enum Images
    {
        FadeImage
    }

    private float _fadeDuration = 0.5f;
    private Image _fadeImage;

    public override bool Init()
    {
        if (base.Init()) return false;

        BindImage(typeof(Images));

   
        _fadeImage = GetImage((int)Images.FadeImage);

        return true;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, Action onComplete)
    {
        float elapsed = 0f;
        Color color = _fadeImage.color; 

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / _fadeDuration);
            _fadeImage.color = color;

            yield return null;
        }

        color.a = endAlpha;
        _fadeImage.color = color;

        onComplete?.Invoke();
    }

    public void FadeIn(Action onComplete = null)
    {
        _fadeImage.gameObject.SetActive(true); 
        StartCoroutine(Fade(1f, 0f, () =>
        {
            _fadeImage.gameObject.SetActive(false); 
            onComplete?.Invoke();
        }));
    }

    public void FadeOut(Action onComplete = null)
    {
        _fadeImage.gameObject.SetActive(true); 
        StartCoroutine(Fade(0f, 1f, onComplete)); //그다음 챕터 loading 을 위한 action
    }
}
