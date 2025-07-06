using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingTrigger : MonoBehaviour
{
    [Header("Owner")]
    [SerializeField]
    private PZ_Parking _owner;

     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>()!=null)
        {
            if (!_owner.IsInsidePlayer()) OnEnterParkingZone();
            else OnExitParkingZone();
        }
    }
    void OnEnterParkingZone()
    {
        WideViewCamera();
        _owner.SettingInsidePlayer(true);
    }
    void OnExitParkingZone()
    {
        ResetCamera();
        _owner.SettingInsidePlayer(false);
    }
    void WideViewCamera()
    {
        Managers.Object.PlayerCamera.SettingCameraOrthoSize(6.0f);
    }
    void ResetCamera()
    {
        Managers.Object.PlayerCamera.ResetCameraOrthoSize();
    }
    
}
