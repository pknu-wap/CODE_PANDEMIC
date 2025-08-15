using UnityEngine;
using System.Collections;

public class UI_StaminaBar : UI_Base
{
    enum Images
    {
        PlayerStaminaBar
    }

    [SerializeField, Range(1f, 20f)]
    private float _smoothSpeed = 8f;

    RectTransform _stamina;
    private float _originWidth;
    private Coroutine _staminaRoutine;

    private float _velocity = 0f;

    public override bool Init()
    {
        if (base.Init() == false) return false;

        BindImage(typeof(Images));
        _stamina = GetImage((int)Images.PlayerStaminaBar).rectTransform;
        _originWidth = _stamina.sizeDelta.x;
        return true;
    }

    public void UpdateStamina(float ratio)
    {
        if (_stamina == null) return;

        ratio = Mathf.Clamp01(ratio);

        if (_staminaRoutine != null)
            StopCoroutine(_staminaRoutine);

        _staminaRoutine = StartCoroutine(SmoothUpdateStamina(ratio));
    }

    private IEnumerator SmoothUpdateStamina(float targetRatio)
    {
        float targetWidth = _originWidth * targetRatio;
        _velocity = 0f;

        while (true)
        {
            float currentWidth = _stamina.sizeDelta.x;
            float newWidth = Mathf.SmoothDamp(currentWidth, targetWidth, ref _velocity, 1f / _smoothSpeed);

            Vector2 size = _stamina.sizeDelta;
            size.x = newWidth;
            _stamina.sizeDelta = size;

     
            if (Mathf.Abs(newWidth - targetWidth) < 0.05f)
                break;

            yield return null;
        }

     
        Vector2 finalSize = _stamina.sizeDelta;
        finalSize.x = targetWidth;
        _stamina.sizeDelta = finalSize;

        _staminaRoutine = null;
    }
}
