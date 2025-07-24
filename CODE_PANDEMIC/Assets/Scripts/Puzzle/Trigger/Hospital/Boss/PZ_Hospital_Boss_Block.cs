using UnityEngine;
using System.Collections;

public class PZ_Hospital_Boss_Block : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rd;
   
    public void StartMove()
    {
        StartCoroutine(DropTheCars());
    }

    private void OnEnable()
    {
        Managers.Event.Subscribe("OnBossClear", OnBossClear);
    }

    private void OnDisable()
    {
        Managers.Event.Unsubscribe("OnBossClear", OnBossClear);
    }

    private void OnBossClear(object obj)
    {
        Destroy(gameObject);
    }

    private IEnumerator DropTheCars()
    {
       
        Vector3 start = new Vector2(6.16f, -20);
        Vector3 middle = new Vector2(6.16f, -20.5f);
        Vector3 end = new Vector2(6.16f, -21);

        _rd.position = start;

        float currentTime = 0;
        float currentPercent = 0;
        float moveDuration = 0.1f;

        while (currentPercent <= 1)
        {
            currentTime += Time.fixedDeltaTime;
            currentPercent = currentTime / moveDuration;

            _rd.position = Vector2.Lerp(start, end, currentPercent);

            yield return new WaitForFixedUpdate();
        }
       
        currentTime = 0;
        currentPercent = 0;

        while (currentPercent <= 1)
        {
            currentTime += Time.fixedDeltaTime;
            currentPercent = currentTime / moveDuration;

            _rd.position = Vector2.Lerp(end, start, currentPercent);

            yield return new WaitForFixedUpdate();
        }

        currentTime = 0;
        currentPercent = 0;
        moveDuration = 0.05f;

        while (currentPercent <= 1)
        {
            currentTime += Time.fixedDeltaTime;
            currentPercent = currentTime / moveDuration;

            yield return new WaitForFixedUpdate();
        }
      
        currentTime = 0;
        currentPercent = 0;
        moveDuration = 0.1f;

        while (currentPercent <= 1)
        {
            currentTime += Time.fixedDeltaTime;
            currentPercent = currentTime / moveDuration;

            _rd.position = Vector2.Lerp(start, end, currentPercent);

            yield return new WaitForFixedUpdate();
        }
       
        currentTime = 0;
        currentPercent = 0;

        while (currentPercent <= 1)
        {
            currentTime += Time.fixedDeltaTime;
            currentPercent = currentTime / moveDuration;

            _rd.position = Vector2.Lerp(end, middle, currentPercent);

            yield return new WaitForFixedUpdate();
        }
      
        currentTime = 0;
        currentPercent = 0;
        moveDuration = 0.05f;

        while (currentPercent <= 1)
        {
            currentTime += Time.fixedDeltaTime;
            currentPercent = currentTime / moveDuration;

            yield return new WaitForFixedUpdate();
        }
       
        currentTime = 0;
        currentPercent = 0;
        moveDuration = 0.1f;

        while (currentPercent <= 1)
        {
            currentTime += Time.fixedDeltaTime;
            currentPercent = currentTime / moveDuration;

            _rd.position = Vector2.Lerp(middle, end, currentPercent);

            yield return new WaitForFixedUpdate();
        }

        Managers.Event.InvokeEvent("SpawnBossMonster");
    }
}