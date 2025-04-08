using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private float _maxRealHp = 100f; // 실제 최대 체력, 임시 값
    private float _realHp = 100f; // 실제 현재 체력, 임시 값
    private float _effectHp = 100f; // 체력 증감소 효과를 줄 체력, 임시 값

    public float MaxRealHp { get { return _maxRealHp; } }
    public float RealHp { get { return _realHp; } }
    public float EffectHp { get { return _effectHp; } }

    public void OnDamaged(float damageValue)
    {
        float damagedHp = Mathf.Clamp(RealHp - damageValue, 0, MaxRealHp);

        if (damagedHp <= 0)
        {
            return;
        }
    }
}