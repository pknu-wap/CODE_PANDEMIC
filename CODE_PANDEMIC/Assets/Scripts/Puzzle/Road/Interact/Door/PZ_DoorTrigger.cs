using UnityEngine;
using System;

public class PZ_DoorTrigger : MonoBehaviour
{
    public static event Action CloseWorksiteDoor;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        CloseWorksiteDoor?.Invoke();
        Destroy(gameObject);
    }
}