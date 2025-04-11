using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class ObjectManager : MonoBehaviour
{
     
    private int _leftMonsters;
    private int _leftPuzzles;
    //Dictionary<SpawnerBase int>_leftMonsterSpawners;
    //PlayerSpawner _playerSpawn;
    //StageExit 
    //List<Puzzles>����� 
    public bool Loaded { get; private set; }
    public GameObject MapObject;
    public PlayerStatus Player { get; set; }
   
  
  
    public IEnumerator CoLoadStageData(StageData stageData)
    {
        Loaded = false;
        _leftMonsters = 0;
        _leftPuzzles = 0;

        // �� �ε�
        bool mapLoaded = false;
        Managers.Resource.Instantiate(stageData.MapAddress, null, (obj) =>
        {
            MapObject = obj;
            mapLoaded = true;
        });
        while (!mapLoaded) yield return null;

        RegisterSpawners();
        SpawnPlayer();
        SpawnMonster();

        Loaded = true;
    }

    public void SpawnMonster()
    {
        //TODO SPAWNER ���� 
    }
    public void ReleaseMonster()
    {
        //TODO WHEN MONSTER DIED
    }
    public void OnStageCleared()
    {
        ResetStageObjects();
    }
    private void ResetStageObjects()
    {
         //TODO scene �� ��ȯ�� ������Ʈ �����ִ°� ���ı� 
    }
    public void RegisterSpawners()
    {
        //TODO WHEN SPAWNER COMPLETE
    }
    public void UnRegisterSpawners()
    {
        //TODO WHEN SPAWNER COMPLETE
    }
    public void SpawnPlayer()
    {
        Managers.Resource.Instantiate("Player", null, (obj) =>
        {
            Player = obj.GetComponent<PlayerStatus>();
            obj.GetOrAddComponent<TempDamage>();
            obj.GetComponent<TempDamage>().Status = Player;

            Player.SetInfo();
        });
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
        Debug.Log("��� ���� Ŭ�����, ���� ���ÿ�");

    }
       
        
}
