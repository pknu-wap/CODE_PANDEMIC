using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    private int _maxHp = 100;
    [SerializeField] private float _currentHp;
    private float _effectHp;

    public delegate void HpEffectUpdateDelegate(float realRatio, float effectRatio);
    private HpEffectUpdateDelegate _onHpEffectUpdated;

    private Coroutine _damageEffectCoroutine;

    public float MaxRealHp => _maxHp;
    public float RealHp => _currentHp;
    public float EffectHp => _effectHp;

    public void SetInfo()
    {
        _currentHp = _maxHp;
        _effectHp = _maxHp;

        if (Managers.UI.SceneUI is UI_GameScene gameSceneUI && gameSceneUI.StatusBar != null)
        {
            SetHpEffectDelegate(gameSceneUI.StatusBar.UpdateHp);
        }

        _onHpEffectUpdated?.Invoke(_currentHp / _maxHp, _effectHp / _maxHp);
    }

    public void SetHpEffectDelegate(HpEffectUpdateDelegate updateMethod)
    {
        _onHpEffectUpdated = updateMethod;
    }

    public void OnDamaged(float damageValue)
    {
        _currentHp = Mathf.Clamp(_currentHp - damageValue, 0, _maxHp);

        if (_currentHp <= 0)
        {
            // 사망 처리
            return;
        }

        if (_damageEffectCoroutine != null)
            StopCoroutine(_damageEffectCoroutine);

        _damageEffectCoroutine = StartCoroutine(OnDamagedEffect());
    }

    public void OnHealed(float healValue)
    {
        _currentHp = Mathf.Clamp(_currentHp + healValue, 0, _maxHp);
        _effectHp = Mathf.Max(_effectHp, _currentHp);

        _onHpEffectUpdated?.Invoke(_currentHp / _maxHp, _effectHp / _maxHp);
    }

    private IEnumerator OnDamagedEffect()
    {
        float duration = 0.5f;
        float timer = 0f;
        float startHp = _effectHp;
        float targetHp = _currentHp;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            _effectHp = Mathf.SmoothStep(startHp, targetHp, t);  

            _onHpEffectUpdated?.Invoke(_currentHp / _maxHp, _effectHp / _maxHp);
            yield return null;
        }

        _effectHp = _currentHp;
        _onHpEffectUpdated?.Invoke(_currentHp / _maxHp, _effectHp / _maxHp);
    }
}
