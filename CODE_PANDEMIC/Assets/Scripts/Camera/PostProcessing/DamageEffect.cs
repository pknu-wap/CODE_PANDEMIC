using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;
using System.Collections;

public class DamageEffect : MonoBehaviour
{
    Volume _volume;
    Vignette _vignette;

    Coroutine _fadeCoroutine;
    float _fadeDuration = 0.5f;
    float _startIntensity = 0.5f;

    private void Start()
    {
        
        _volume = GetComponent<Volume>();
        if (_volume.profile.TryGet(out _vignette))
        {
            _vignette.active = false;
            _vignette.intensity.value = 0f;
        }
        else
        {
            Debug.LogError("Vignette not found in Volume Profile");
        }
    }

    private void OnEnable()
    {
        Managers.Event.Subscribe("RiskDamage", CallEffect);
        Managers.Event.Subscribe("ResetIntensity", ResetIntensity);
    }

    private void OnDisable()
    {
        Managers.Event.Unsubscribe("RiskDamage", CallEffect);
        Managers.Event.Unsubscribe("ResetIntensity", ResetIntensity);
    }

    private void ResetIntensity(object obj)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _vignette.intensity.value = 0f;
        _vignette.active = false;
    }

    private void CallEffect(object obj)
    {
        if (_fadeCoroutine != null)
            return;

        _fadeCoroutine = StartCoroutine(FadeOut(_startIntensity));
    }

    private IEnumerator FadeOut(float startIntensity)
    {
        _vignette.intensity.value = startIntensity;
        _vignette.active = true;

        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _fadeDuration;
            _vignette.intensity.value = Mathf.Lerp(startIntensity, 0f, t);
            yield return null;
        }

        _vignette.intensity.value = 0f;
        _vignette.active = false;
        _fadeCoroutine = null;
    }
}
