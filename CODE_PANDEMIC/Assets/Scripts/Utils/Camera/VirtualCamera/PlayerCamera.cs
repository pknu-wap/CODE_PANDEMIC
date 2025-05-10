using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerCamera : VirtualCameraBase
{
   
    Vector3 _playerCameraOffset;
    
    private CinemachineTransposer _transposer;
    
    private CinemachineConfiner2D _confiner;
    private Transform playerTransform;

    protected override bool Init()
    {
        if (base.Init() == false) return false;
        _transposer=GetComponent<CinemachineTransposer>();  
        _confiner=GetComponent<CinemachineConfiner2D>();
        return true;

    }

    public void Setup(Transform obj, PolygonCollider2D cameraLimit)
    {
        if (!Init()) return;
        _playerCameraOffset = new Vector3(0, 0, -10);
        playerTransform = obj;
        _camera.transform.rotation = Quaternion.identity;
        _camera.Follow = playerTransform;

        var vcam = obj.GetComponent<CinemachineVirtualCamera>();
        if (vcam != null)
            vcam.Priority = Define.PlayerCamera;

        _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
        if (_transposer != null)
        {
            _transposer.m_FollowOffset = _playerCameraOffset;
        }

        _confiner = _camera.GetComponent<CinemachineConfiner2D>();
        if (_confiner != null)
        {
            _confiner.m_BoundingShape2D = cameraLimit;
            StartCoroutine(SetUpCameraDelayed());
        }
    }
    IEnumerator SetUpCameraDelayed()
    {
        yield return null;
        _confiner.InvalidateCache();
    }
    public void SnapToTargetImmediately()
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();
        var transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
          
            float originalXDamping = transposer.m_XDamping;
            float originalYDamping = transposer.m_YDamping;
            float originalZDamping = transposer.m_ZDamping;

            
            transposer.m_XDamping = 0;
            transposer.m_YDamping = 0;
            transposer.m_ZDamping = 0;

         
            vcam.ForceCameraPosition(vcam.Follow.position + transposer.m_FollowOffset, vcam.transform.rotation);

           
            StartCoroutine(ResetDampingNextFrame(transposer, originalXDamping, originalYDamping, originalZDamping));
        }
    }

    private IEnumerator ResetDampingNextFrame(CinemachineTransposer transposer, float xDamping, float yDamping, float zDamping)
    {
        yield return null;

        transposer.m_XDamping = xDamping;
        transposer.m_YDamping = yDamping;
        transposer.m_ZDamping = zDamping;
    }




}
