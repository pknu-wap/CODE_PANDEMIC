using UnityEngine;

public class FlameHitbox : MonoBehaviour
{
    [SerializeField] private float damageInterval = 0.2f;

    private float _damageTimer = 0f;
    private int _damagePerSecond = 0;

    public void SetInfo(WeaponData data)
    {
        _damagePerSecond = data.Damage;
        Debug.Log($"[FlameHitbox] SetInfo 호출됨 - DamagePerSecond: {_damagePerSecond}");//지금 호출 안됨
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _damageTimer += Time.deltaTime;

        if (_damageTimer >= damageInterval)
        {
            AI_Base enemy = other.GetComponent<AI_Base>();
            if (enemy != null)
            {
                int damage = Mathf.CeilToInt(_damagePerSecond * damageInterval);
                enemy.TakeDamage(damage);
                Debug.Log($"[FlameHitbox] 적에게 데미지 줌: {damage}");//이건 됨
            }

            _damageTimer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _damageTimer = 0f;
    }
}