using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    private int _maxRealHp = 100;
    private float _realHp;
    private float _effectHp;

    public delegate void HpEffectUpdateDelegate(float realRatio, float effectRatio);
    private HpEffectUpdateDelegate _onHpEffectUpdated;

    public float MaxRealHp => _maxRealHp;
    public float RealHp => _realHp;
    public float EffectHp => _effectHp;

    public void SetInfo()
    {
        _realHp = _maxRealHp;
        _effectHp = _maxRealHp;

        // UI_GameScene이 존재하면 델리게이트 연결
        if (Managers.UI.SceneUI is UI_GameScene gameSceneUI && gameSceneUI.StatusBar != null)
        {
            SetHpEffectDelegate(gameSceneUI.StatusBar.UpdateHp);
        }

        _onHpEffectUpdated?.Invoke(_realHp / _maxRealHp, _effectHp / _maxRealHp);
    }

    public void SetHpEffectDelegate(HpEffectUpdateDelegate updateMethod)
    {
        _onHpEffectUpdated = updateMethod;
    }

    public void OnDamaged(float damageValue)
    {
        _realHp = Mathf.Clamp(_realHp - damageValue, 0, _maxRealHp);

        if (_realHp <= 0)
        {
            // 사망 처리 등
            return;
        }

        StartCoroutine(OnDamagedEffect());
    }

    public void OnHealed(float healValue)
    {
        _realHp = Mathf.Clamp(_realHp + healValue, 0, _maxRealHp);
        _effectHp = Mathf.Max(_effectHp, _realHp);

        _onHpEffectUpdated?.Invoke(_realHp / _maxRealHp, _effectHp / _maxRealHp);
    }

    private IEnumerator OnDamagedEffect()
    {
        float duration = 0.5f;
        float timer = 0f;
        float startHp = _effectHp;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            _effectHp = Mathf.Lerp(startHp, _realHp, t);

            _onHpEffectUpdated?.Invoke(_realHp / _maxRealHp, _effectHp / _maxRealHp);
            yield return null;
        }

        _effectHp = _realHp;
        _onHpEffectUpdated?.Invoke(_realHp / _maxRealHp, _effectHp / _maxRealHp);
    }
}
