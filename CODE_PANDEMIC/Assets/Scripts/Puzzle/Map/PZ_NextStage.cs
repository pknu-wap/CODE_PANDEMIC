using UnityEngine;
using System.Collections;

public class PZ_NextStage : MonoBehaviour
{
    [SerializeField] private bool _isNextStage = true;
    private bool _isTriggered = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponentInParent<PlayerController>())
        {
            return;
        }

        if (_isTriggered)
        {
            return;
        }

        StartCoroutine(ChangeStage());

        _isTriggered = true;
    }

    private IEnumerator ChangeStage()
    {
        yield return new WaitForSeconds(0.8f);

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