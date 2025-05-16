using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraBase : MonoBehaviour
{
    protected CinemachineVirtualCamera _camera;
    private bool _init = false;
    private void Awake()
    {
        Init(); 
    }
    protected virtual bool Init()
    {
        if(_init)return true;

        _camera = GetComponent<CinemachineVirtualCamera>();
        if (_camera == null) return false;

        _init = true;
        return true;
    }
}
