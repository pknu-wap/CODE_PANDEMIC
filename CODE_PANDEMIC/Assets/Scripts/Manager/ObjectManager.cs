using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class ObjectManager : MonoBehaviour
{
    private int _leftPuzzles;
    private int _leftSpawners;
    public bool Loaded { get; private set; }
    public StageController MapObject { get; private set; }
    public PlayerStatus Player { get; set; }

     
    public IEnumerator CoLoadStageData(StageData stageData)
    {
        Loaded = false;
        _leftPuzzles = 0;
        _leftSpawners = 0;


        bool mapLoaded = false;
        Managers.Resource.Instantiate(stageData.MapAddress, null, (obj) =>
        {
           MapObject = obj.GetComponent<StageController>();
           MapObject.SetInfo(stageData);
           mapLoaded = true;
        });
        while (!mapLoaded) yield return null;
        while(Player==null) yield return null;
        while (_leftSpawners < stageData.Spawners.Count) yield return null;
        Loaded = true;
    }

    public void ResetStage()
    {
        _leftPuzzles = 0;
        _leftSpawners = 0;
        Destroy(Player.gameObject);
        Player = null;
        Destroy(MapObject.gameObject);
        MapObject = null;
    }
    public void RegisterPlayer(PlayerStatus player)
    {
        Player= player;
    }
    public void RegisterSpawners(SpawnBase spawner)
    {
        _leftSpawners++;
    }
    public void UnRegisterSpawners(SpawnBase spawner)
    {
        _leftSpawners--;
    }
    
    public void RegisterPuzzles()
    {
        _leftPuzzles++;
    }
    
    public void UnRegisterPuzzles()
    {
        _leftPuzzles--;
    }
  
}
