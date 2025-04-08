using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private float _maxRealHp = 100f; // ���� �ִ� ü��, �ӽ� ��
    private float _realHp = 100f; // ���� ���� ü��, �ӽ� ��
    private float _effectHp = 100f; // ü�� ������ ȿ���� �� ü��, �ӽ� ��

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