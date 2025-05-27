using UnityEngine;

public class FlameHitbox : MonoBehaviour
{
    [SerializeField] private float damageInterval = 0.05f;
    private float _damageTimer = 0f;
    private int _damagePerSecond = 0;



    public void SetInfo(WeaponData data)
    {
        _damagePerSecond = data.Damage;
        Debug.Log("[FlameHitbox] SetInfo 호출, Damage: " + _damagePerSecond);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_damagePerSecond <= 0) return;

        _damageTimer += Time.deltaTime;
        if (_damageTimer >= damageInterval)
        {
            AI_Base enemy = other.GetComponent<AI_Base>();
            if (enemy != null)
            {
                int damage = Mathf.CeilToInt(_damagePerSecond * damageInterval);
                Debug.Log("[FlameHitbox] 적 타격! Damage: " + damage + ", 대상: " + enemy.name);
                enemy.TakeDamage(damage);
            }
            _damageTimer = 0f;
        }
    }

    private void OnDisable()
    {
        _damageTimer = 0f;
        _damagePerSecond = 0;
        Debug.Log("[FlameHitbox] OnDisable 호출 – 타이머 리셋, 데미지 0");
    }

}
