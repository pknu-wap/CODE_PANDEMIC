using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDamageEffect : MonoBehaviour
{
    [ColorUsage(true,true)]
    [SerializeField] private Color _flashColor = Color.white;
    private float _flashTime = 0.25f;
    [SerializeField] AnimationCurve _flashSpeedCurve;

    SpriteRenderer _spriteRenderer;
    Material _material;

    private Coroutine _damageFlashCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
    }
    private void OnDisable()
    {
        if (_damageFlashCoroutine != null)
        {
            StopCoroutine(_damageFlashCoroutine);
            _damageFlashCoroutine = null;
        }
    }
    public void CallDamageFlash()
    {
        if (_damageFlashCoroutine != null) return;
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }
    private void SetFlashColor()
    {
        _material?.SetColor("_FlashColor", _flashColor);
    }
    private void SetFlashAmount(float amount)
    {
        _material?.SetFloat("_FlashAmount", amount);
    }
    private IEnumerator DamageFlasher()
    {
        SetFlashColor();

        float currentFlashAmount = 0.0f;
        float elapsedTime = 0.0f;
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, _flashSpeedCurve.Evaluate(elapsedTime), elapsedTime / _flashTime);
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
        _damageFlashCoroutine = null;
    }
}
