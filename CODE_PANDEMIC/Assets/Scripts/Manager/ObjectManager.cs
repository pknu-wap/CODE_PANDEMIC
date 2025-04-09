using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //monstercontroller list 
    private int _leftMonsters;
    private int _leftPuzzles;
    //Dictionary<SpawnerBase int>_leftMonsterSpawners;
    //PlayerSpawner _playerSpawn;
    //StageExit 
    //List<Puzzles>����� 
    public GameObject MapObject;
    public PlayerStatus Player { get; set; }
   
    public void  LoadStageData(StageData stageData)
    {
        _leftPuzzles = 0;
        _leftMonsters = 0;
        Managers.Resource.Instantiate(stageData.MapAddress, null, (obj) =>
        {
            Debug.Log("Map");
            SpawnPlayer();
        });
     
    }
   
  
    public void SpawnMonster()
    {

    }
    public void ReleaseMonster()
    {

    }
    public void OnStageCleared()
    {
        ResetStageObjects();
    }
    private void ResetStageObjects()
    {
         // scene �� ��ȯ�� ������Ʈ �����ִ°� ���ı� 
    }
    public void RegisterSpawners()
    {
        
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

    public void UnRegisterSpawners()
    {
       
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
