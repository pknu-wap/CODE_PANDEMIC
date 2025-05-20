using UnityEngine;

public class FlameHitbox : MonoBehaviour
{
    [SerializeField] private float damageInterval = 0.2f;

    private float _damageTimer = 0f;
    private int _damagePerSecond = 0;

    public void SetInfo(WeaponData data)
    {
        _damagePerSecond = data.Damage;
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
            }

            _damageTimer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _damageTimer = 0f;
    }
}
