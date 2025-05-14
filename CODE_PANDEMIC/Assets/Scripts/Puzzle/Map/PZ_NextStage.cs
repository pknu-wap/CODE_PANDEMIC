using UnityEngine;

public class PZ_NextStage : MonoBehaviour
{
    [SerializeField] private bool _isNextStage = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isNextStage)
        {
            Managers.Event.InvokeEvent("NextStage");
        }
        else
        {
            Managers.Event.InvokeEvent("PrevStage");
        }
    }
}