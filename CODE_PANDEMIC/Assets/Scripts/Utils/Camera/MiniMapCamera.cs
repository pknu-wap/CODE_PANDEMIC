using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField]  
    RenderTexture _miniMapTexture;
    Camera _miniMapCam;

    private void Awake()
    {
        _miniMapCam = GetComponent<Camera>();
    }
    void Start()
    {
        _miniMapCam.targetTexture = _miniMapTexture;
        Managers.Event.InvokeEvent("OnMiniMapReady", _miniMapTexture);
    }
}
