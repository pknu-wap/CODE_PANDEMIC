using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShockWave : MonoBehaviour
{
    float _shockWaveTime = 0.75f;
    Coroutine _shockWaveCoroutine;
    Material _material;
    static int _waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");
    private static readonly int _xSizeRatioID = Shader.PropertyToID("_XSizeRatio");

   
    private void Awake()
    {
        _material=GetComponent<SpriteRenderer>().material;  
    }
    private void OnEnable()
    {
        if (_material == null)
            _material = GetComponent<Renderer>().material;

        float aspect = (float)Screen.width / Screen.height;
        _material.SetFloat(_xSizeRatioID, aspect);
    }
    private void Start()
    {
        
        //gameObject.SetActive(false);
    }
   
    public void CallShockWave(int repeatCount = 1, float delayBetween = 0.1f)
    {
        if (_shockWaveCoroutine != null)
            StopCoroutine(_shockWaveCoroutine);

        _shockWaveCoroutine = StartCoroutine(ShockWaveMultiple(repeatCount, delayBetween));
    }

    IEnumerator ShockWaveMultiple(int repeatCount, float delay)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            yield return StartCoroutine(ShockWaveAction(0f, 1f));
            yield return CoroutineHelper.WaitForSeconds(delay);
        }
    }

    IEnumerator ShockWaveAction(float startPos, float endPos)
    {
        _material.SetFloat(_waveDistanceFromCenter, startPos);
        float lerpedAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < _shockWaveTime)
        {
            lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime / _shockWaveTime);
            _material.SetFloat(_waveDistanceFromCenter, lerpedAmount);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
