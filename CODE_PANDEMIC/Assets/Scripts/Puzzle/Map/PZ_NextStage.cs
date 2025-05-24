using UnityEngine;
using System.Collections;

public class PZ_NextStage : MonoBehaviour
{
    [SerializeField] private bool _isNextStage = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponentInParent<PlayerController>())
        {
            return;
        }

        StartCoroutine(ChangeStage());
    }

    private IEnumerator ChangeStage()
    {
        yield return new WaitForSeconds(1f);

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