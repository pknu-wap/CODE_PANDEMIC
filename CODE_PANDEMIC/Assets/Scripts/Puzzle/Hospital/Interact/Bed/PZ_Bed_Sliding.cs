using UnityEngine;
using System.Collections;

public class PZ_Bed_Sliding : PZ_Interact_NonSpawn
{
    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        StartCoroutine(MoveBed());
    }

    private IEnumerator MoveBed()
    {
        float targetPosX = transform.position.x - 3;

        float currentTime = 0f;
        float currentPercent = 0f;
        float moveDuration = 20f;

        while (currentPercent < 1)
        {
            currentTime += Time.deltaTime;
            currentPercent = currentTime / moveDuration;

            Vector3 tempPos = transform.position;
            tempPos.x = Mathf.Lerp(transform.position.x, targetPosX, currentPercent);

            transform.position = tempPos;

            yield return null;
        }
    }
}