using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    Volume _volume;
    Vignette _vignette;

    private void Start()
    {
        _volume = GetComponent<Volume>();
        if (_volume.profile.TryGet(out _vignette))
        {
            _vignette.active = false;
        }
        else
        {
            Debug.LogError("Error, Vignette not found in Volume Profile");
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
        Managers.Event.Subscribe("ResetIntensity", ResetIntensity);

    }
    private void ResetIntensity(object obj)
    {
        _vignette.active = true;
        _vignette.intensity.value = 0;
        _vignette.active = false;

    }
    private void CallEffect(object obj)
    {
        float hpPercent = (float)obj;

        if (hpPercent <= 0.5f)
        {
            float t = Mathf.InverseLerp(0.5f, 0f, hpPercent);
            float intensity = Mathf.Lerp(0f, 0.7f, t);
            _vignette.intensity.value = intensity;
            _vignette.active = true;
        }
        else
        {
            _vignette.active = false;
        }
    }
}