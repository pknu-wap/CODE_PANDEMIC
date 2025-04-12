using System.Collections;
using System.Collections.Generic;
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

        // ¸Ê ·Îµå
        bool mapLoaded = false;
        Managers.Resource.Instantiate(stageData.MapAddress, null, (obj) =>
        {
            MapObject = obj;
            mapLoaded = true;
        });
        while (!mapLoaded) yield return null;


        SpawnSpawners(stageData.Spawners);
        while(Player==null) yield return null;
        while (_spawnList.Count < stageData.Spawners.Count) yield return null;
        Loaded = true;
    }

    public void SpawnSpawners(List<SpawnerInfoData> spawners)
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            var spawnerData = spawners[i]; 
            Managers.Resource.Instantiate(spawnerData.Name, null, (obj) =>
            {
                obj.transform.position = new Vector3(spawnerData.Pos.x,spawnerData.Pos.y, 0);
                AI_Spawner monsterSpawner = obj.GetComponent<AI_Spawner>();
                if (monsterSpawner != null)
                {
                    Debug.Log(spawnerData.ID); 
          
                    monsterSpawner.SetInfo(spawnerData.ID);
                    monsterSpawner.SpawnObjects();
                }
                else
                {
                    obj.GetComponent<SpawnBase>().SpawnObjects();
                }
              
            });
        }
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
        Debug.Log(_spawnList.Count);
        _spawnList.Add(spawner);
    }
    public void UnRegisterSpawners(SpawnBase spawner)
    {
        _spawnList.Remove(spawner);
    }
    
    public void RegisterPuzzles()
    {
        _leftPuzzles++;
        Debug.Log($"{_leftPuzzles}");
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
        Debug.Log("¸ðµç ÆÛÁñ Å¬¸®¾îµÊ, ¹®À» ¿©½Ã¿À");

    }
    public void ResetStage()
    {
       
    }


}
