using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Range(1, 10)]
    private float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
