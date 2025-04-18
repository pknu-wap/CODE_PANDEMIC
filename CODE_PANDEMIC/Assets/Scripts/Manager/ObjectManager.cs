using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class ObjectManager : MonoBehaviour
{
    
    private int _leftPuzzles;
    private List<SpawnBase> _spawnList=new List<SpawnBase> ();
    //StageExit 
    //List<Puzzles>ÆÛÁñµé 
    public bool Loaded { get; private set; }
    public GameObject MapObject;
    public PlayerStatus Player { get; set; }
     
    public IEnumerator CoLoadStageData(StageData stageData)
    {
        Loaded = false;
        _leftPuzzles = 0;

       
        bool mapLoaded = false;
        Managers.Resource.Instantiate(stageData.MapAddress, null, (obj) =>
        {
            MapObject = obj;
            obj.GetComponent<StageController>().SetInfo(stageData);
            mapLoaded = true;
        });
        while (!mapLoaded) yield return null;
        while(Player==null) yield return null;
        while (_spawnList.Count < stageData.Spawners.Count) yield return null;
        Loaded = true;
    }

    public void OnStageCleared()
    {
        ResetStageObjects();
    }
    private void ResetStageObjects()
    {
        _leftPuzzles = 0;
        foreach (var spawner in _spawnList)
        {
            Destroy(spawner.gameObject);
        }
        Destroy(Player.gameObject);
        Destroy(MapObject);
    }
    public void RegisterPlayer(PlayerStatus player)
    {
        Player= player;
    }
    public void RegisterSpawners(SpawnBase spawner)
    {
        _spawnList.Add(spawner);
    }
    public void UnRegisterSpawners(SpawnBase spawner)
    {
        _spawnList.Remove(spawner);
    }
    
    public void RegisterPuzzles()
    {
        _leftPuzzles++;
    }

    
    public void UnRegisterPuzzles()
    {
        _leftPuzzles--;
        if (_leftPuzzles <= 0)
        {
            OnAllPuzzlesCleared();
        }
    }
    private void OnAllPuzzlesCleared()
    {
        Managers.Event.InvokeEvent("StageClear");

    }
    public void ResetStage()
    {
        _spawnList.Clear();
        Destroy(Player.gameObject);
        Player = null; 
        Destroy(MapObject);
        MapObject = null;
    }
}
