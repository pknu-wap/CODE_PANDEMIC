using UnityEngine;

public class PZ_NextStage : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Managers.Event.InvokeEvent("NextStage");
    }
}