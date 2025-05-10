using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlockLight : MonoBehaviour
{ 
    Light2D _light;
    private void Start()
    {
        _light = GetComponent<Light2D>();
        gameObject.SetActive(false); //불필요한 연산 줄이기       
    }
    public void LightBlock(Action onComplete = null)
    {
        StartCoroutine(LightFadeIn(onComplete));
    }
    IEnumerator LightFadeIn(Action onComplete=null)
    {
        float duration = 1.5f; 
        float time = 0f;

        float startIntensity = 0f;
        float targetIntensity = 1f; 

        // 초기화
        _light.intensity = startIntensity;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            _light.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            yield return null;
        }

        _light.intensity = targetIntensity;
        onComplete?.Invoke();
    }

}
