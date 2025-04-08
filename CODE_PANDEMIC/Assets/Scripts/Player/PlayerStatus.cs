using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    private float _maxRealHp = 100f; // 실제 최대 체력, 임시 값
    private float _realHp = 100f; // 실제 현재 체력, 임시 값
    private float _effectHp = 100f; // 체력 감소 효과를 줄 체력, 임시 값

    public float MaxRealHp { get { return _maxRealHp; } }
    public float RealHp { get { return _realHp; } }
    public float EffectHp { get { return _effectHp; } }

    // 현재 체력의 정보를 가져오는 함수
    public float GetCurrentHpPercent()
    {
        return _realHp / _maxRealHp;
    }

    // 데미지를 받는 함수
    public void OnDamaged(float damageValue)
    {
        _realHp = Mathf.Clamp(RealHp - damageValue, 0, MaxRealHp); // 조정

        if (_realHp <= 0)
        {
            // 여기에 플레이어 사망 처리
            return;
        }

        // 여기에 데미지 받았을 때 효과 처리

        StartCoroutine(OnDamagedEffect());
    }

    // 체력 회복을 하는 함수
    public void OnHealed(float healValue)
    {
        _realHp = Mathf.Clamp(RealHp + healValue, 0, MaxRealHp); // 조정
    }

    // 데미지를 받았을 때 이펙트 체력이 서서히 떨어지게 만드는 함수
    private IEnumerator OnDamagedEffect()
    {
        float currentTime = 0f;
        float currentPercent = 0f;
        float effectDuration = 0.5f; // 몇 초 동안 이펙트가 나타나는지

        while (currentPercent < 1)
        {
            currentTime += Time.deltaTime;
            currentPercent = currentTime / effectDuration;

            _effectHp = Mathf.Lerp(EffectHp, RealHp, currentPercent); // 보간

            yield return null;
        }
    }
}