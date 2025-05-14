using UnityEngine;

public class FlameHitbox : MonoBehaviour
{
    [SerializeField] private float damagePerSecond = 5.0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        AI_Base enemy = other.GetComponent<AI_Base>();
        if (enemy != null)
        {
            enemy.TakeDamage((int)(damagePerSecond * Time.deltaTime));
        }
    }
}
