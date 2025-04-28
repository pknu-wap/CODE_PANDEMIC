using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
   
    Vector3 _playerCameraOffset;
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField]
    private CinemachineTransposer _transposer;
    [SerializeField]
    private CinemachineConfiner2D _confiner;
    private Transform playerTransform;
   
  

    public void Setup(Transform obj, PolygonCollider2D cameraLimit)
    {
        _playerCameraOffset = new Vector3(0, 0, -10);
        playerTransform = obj;
        _virtualCamera.transform.rotation = Quaternion.identity;
        _virtualCamera.Follow = playerTransform;

        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (_transposer != null)
        {
            _transposer.m_FollowOffset = _playerCameraOffset;
        }

        // 여기! confiner를 직접 GetComponent로 찾는다
        _confiner = _virtualCamera.GetComponent<CinemachineConfiner2D>();
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

}
