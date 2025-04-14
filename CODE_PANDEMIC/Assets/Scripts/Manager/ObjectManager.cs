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

       
        bool mapLoaded = false;
        Managers.Resource.Instantiate(stageData.MapAddress, null, (obj) =>
        {
            MapObject = obj;
            mapLoaded = true;
        });
        while (!mapLoaded) yield return null;


        SpawnSpawners(stageData.Spawners);
        SpawnPuzzles(stageData.Puzzles);
        while(Player==null) yield return null;
        while (_spawnList.Count < stageData.Spawners.Count) yield return null;
        Loaded = true;
    }

    public void SpawnSpawners(List<SpawnerInfoData> spawners)
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            SpawnerInfoData spawnerData = spawners[i]; 
            Managers.Resource.Instantiate(spawnerData.Name, null, (obj) =>
            {
                obj.transform.position = new Vector3(spawnerData.Pos.x,spawnerData.Pos.y, 0);
                AI_Spawner monsterSpawner = obj.GetComponent<AI_Spawner>();
                if (monsterSpawner != null)
                {
                  
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
    public void SpawnPuzzles(List<int>Puzzles)
    {
        for(int i= 0 ; i<Puzzles.Count; i++)
        {
            if (!Managers.Data.Puzzles.TryGetValue(Puzzles[i], out PuzzleData data))
            {
                Debug.LogError($"Puzzle ID {Puzzles[i]} not found in data.");
                continue;
            }
           
            Managers.Resource.Instantiate(data.Prefab, null, (obj) =>
            {
                obj.transform.position = data.Pos;
                PZ_Puzzle_Item item =obj.GetComponent<PZ_Puzzle_Item>();
                if (item != null)
                {
                    item.SetInfo(data);
                    item.SettingPuzzle();
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
        

    }
    public void ResetStage()
    {
       
    }


}
