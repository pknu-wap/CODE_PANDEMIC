using UnityEngine;
using System.Collections;

public enum PlayerState
{
    Idle,
    Move,
    Invincible,
    Dead
}

public class PlayerStatus : MonoBehaviour
{
    private PlayerController _playerController;

    // 체력 정보
   private int _maxHp = 100;
    [SerializeField] private float _currentHp;
    private float _effectHp;

    // 데미지 효과
    public delegate void HpEffectUpdateDelegate(float realRatio, float effectRatio);
    private HpEffectUpdateDelegate _onHpEffectUpdated;

    private Coroutine _damageEffectCoroutine;

    public float MaxRealHp => _maxHp;
    public float RealHp => _currentHp;
    public float EffectHp => _effectHp;

    // 초기화
    public void SetInfo()
    {
        _currentHp = _maxHp;
        _effectHp = _maxHp;
        _playerController = GetComponent<PlayerController>();

        if (Managers.UI.SceneUI is UI_GameScene gameSceneUI && gameSceneUI.StatusBar != null)
        {
         
            SetHpEffectDelegate(gameSceneUI.StatusBar.UpdateHp);
        }
        else       
        StartCoroutine(WaitForUI());

        _onHpEffectUpdated?.Invoke(_currentHp / _maxHp, _effectHp / _maxHp);
    }
        
    IEnumerator WaitForUI()
    {
        while (!(Managers.UI.SceneUI is UI_GameScene ui) || ui.StatusBar == null)
            yield return null;

        SetHpEffectDelegate(Managers.UI.SceneUI.GetComponent<UI_GameScene>().StatusBar.UpdateHp);
    }
    private void OnDisable()
    {
        if (Managers.UI.SceneUI is UI_GameScene gameSceneUI && gameSceneUI.StatusBar != null)
        {
            DisableDelegate(gameSceneUI.StatusBar.UpdateHp);
        }
    }
    public void SetHpEffectDelegate(HpEffectUpdateDelegate updateMethod)
    {
        _onHpEffectUpdated += updateMethod;
    }
    public void DisableDelegate(HpEffectUpdateDelegate updateMethod)
    {
        _onHpEffectUpdated -= updateMethod;
    }

    // 데미지
    public void OnDamaged(GameObject attacker, float damageValue)
    {
        if (_playerController._currentState == PlayerState.Dead || _playerController._currentState == PlayerState.Invincible)
        {
            return;
        }

        _currentHp = Mathf.Clamp(_currentHp - damageValue, 0, _maxHp);
        if(_currentHp<=_maxHp*0.5f) Managers.Event.InvokeEvent("RiskDamage", _currentHp/_maxHp);

        if (_currentHp <= 0)
        {
            OnPlayerDead();
            return;
        }

        if (_damageEffectCoroutine != null) 
            StopCoroutine(_damageEffectCoroutine);

        _damageEffectCoroutine = StartCoroutine(OnDamagedEffect());
    }

    // 체력 회복
    public void OnHealed(float healValue)
    {
        if (_currentHp/_maxHp<=0.5f && (_currentHp+healValue / _maxHp) >  0.5f) Managers.Event.InvokeEvent("ResetIntensity");
        _currentHp = Mathf.Clamp(_currentHp + healValue, 0, _maxHp);
        _effectHp = Mathf.Max(_effectHp, _currentHp);

        _onHpEffectUpdated?.Invoke(_currentHp / _maxHp, _effectHp / _maxHp);
    }

    // 데미지 효과
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
    private void OnPlayerDead()
    {
        Managers.Event.InvokeEvent("OnPlayerDead");
        Managers.Event.InvokeEvent("ResetIntensity");
    }
}