using UnityEngine;
using System.Collections;

public class PZ_Poster : PZ_Interact_Base
{
    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        StartCoroutine(MoveDown());
    }

    private IEnumerator MoveDown()
    {
        float currentTime = 0f;
        float currentPercent = 0f;
        float moveDuration = 1f;
        float targetPosY = transform.position.y - 1;

        while (currentPercent < 1)
        {
            currentTime += Time.deltaTime;
            currentPercent = currentTime / moveDuration;

            Vector3 tempPos = transform.position;
            tempPos.y = Mathf.Lerp(transform.position.y, targetPosY, currentTime);

            transform.position = tempPos;

            yield return null;
        }
    }
}