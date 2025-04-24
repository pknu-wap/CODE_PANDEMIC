using UnityEngine;

public class PZ_FireExtinguisher_Effect : MonoBehaviour
{
    private float _damage = 10f;

    public void OnTriggerEnter(Collider other)
    {
        other.GetComponent<AI_Controller>()?.TakeDamage((int)_damage);
    }
}