using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class PuzzleClearCamera : MonoBehaviour
{
   
    CinemachineVirtualCamera _camera;
    void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>(); 
        
    }
    private void OnDisable()
    {
        Managers.UI.FadeAtOnce();
        Managers.UI.FadeIn();
    }
    private void Start()
    {
        _camera.Priority = Define.None;
        gameObject.SetActive(false);
    }
    public void LookAtDisappear(Action action)
    {
        StartCoroutine(CameraEffect(action));
    }
    
    IEnumerator CameraEffect(Action action)
    {
        Managers.UI.FadeAtOnce();
        _camera.Priority = Define.PuzzleClear;
        yield return null;
        Managers.UI.FadeIn(action); ;

    }



}
