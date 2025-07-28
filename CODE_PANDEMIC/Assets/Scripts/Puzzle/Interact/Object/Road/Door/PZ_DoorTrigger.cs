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

        Destroy(gameObject);
    }
}