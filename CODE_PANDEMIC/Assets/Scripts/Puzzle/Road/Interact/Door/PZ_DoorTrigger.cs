using UnityEngine;
using System;

public class PZ_DoorTrigger : MonoBehaviour
{
    public static event Action CloseWorksiteDoor;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponentInParent<PlayerController>())
        {
            return;
        }

        CloseWorksiteDoor?.Invoke();

        // 여기에 번개 좀비 소환 구현

        Destroy(gameObject);
    }
}