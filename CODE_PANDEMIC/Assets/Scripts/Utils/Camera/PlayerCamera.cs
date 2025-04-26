using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Camera _camera;
    Vector3 _playerCameraOffset;
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera;

    [SerializeField]
    private CinemachineTransposer _transposer;
    private Transform playerTransform;
   
    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
       
    }


    public void Setup(Transform obj)
    {
        _playerCameraOffset= new Vector3(0, 0, -10);
        playerTransform = obj;
        if (_virtualCamera == null)
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        _virtualCamera.transform.rotation = Quaternion.identity;

        if (_virtualCamera != null)
        {
            _virtualCamera.Follow = playerTransform;

            // 여기 추가!
            _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            if (_transposer != null)
            {
                _transposer.m_FollowOffset = _playerCameraOffset;
            }
            else
            {
                Debug.LogError("CinemachineTransposer is missing!");
            }
        }
        else
        {
            Debug.LogError("Virtual Camera is missing!");
        }
    }

}
