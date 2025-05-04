using System.Collections;
using UnityEngine;
using Cinemachine;

public class ObjectManager : MonoBehaviour
{
    private int _leftSpawners;
    private int _leftPuzzles;
    public bool Loaded { get; private set; }
  
    
    private PlayerCamera _playerCamera;
    public StageController MapObject { get; private set; }
    public PlayerStatus Player { get; private set; }

    public IEnumerator CoLoadStage(StageData stageData)
    {
         Loaded = false;
       
        _leftSpawners = 0;
        _leftPuzzles = 0;

        
        // ¸Ê »ý¼º
        bool mapLoaded = false;
        Managers.Resource.Instantiate(stageData.MapAddress, null, (obj) =>
        {
            MapObject = obj.GetComponent<StageController>();
            MapObject.SetInfo(stageData);
            mapLoaded = true;
        });

        while (!mapLoaded) yield return null;

      
        while (Player == null) yield return null;

     
        bool cameraLoaded = false;
        Managers.Resource.Instantiate("PlayerCamera", null, (obj) =>
        {
            var playerCam = obj.GetComponent<PlayerCamera>();
            _playerCamera = playerCam;
            playerCam.Setup(Player.transform, MapObject.CameraLimit);
            cameraLoaded = true;

           
            playerCam.SnapToTargetImmediately();
        });

       

        while (!cameraLoaded) yield return null;
        while (_leftSpawners < stageData.Spawners.Count) yield return null;

        Loaded = true;
    }

    public void ResetStage()
    {
        if (Player != null)
        {
            Managers.Resource.Destroy(Player.gameObject);
            Player = null;
        }

        if (MapObject != null)
        {
            Managers.Resource.Destroy(MapObject.gameObject);
            MapObject = null;
        }
        if(_playerCamera != null)
        {
            Managers.Resource.Destroy(_playerCamera.gameObject);
            _playerCamera = null;
        }
    }

    public void RegisterPlayer(PlayerStatus player)
    {
        Player = player;
    }

    public void RegisterSpawners()
    {
        _leftSpawners++;
        Debug.Log(_leftSpawners);
    }

    public void UnregisterSpawners()
    {
        _leftSpawners--;
    }

    public void RegisterPuzzles()
    {
        _leftPuzzles++;
    }

    public void UnregisterPuzzles()
    {
        _leftPuzzles--;
    }
}
