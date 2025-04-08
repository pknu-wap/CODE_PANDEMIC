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
    //List<Puzzles>퍼즐들 
    public void SpawnPlayer()
    { 
        //맵 로딩되면 거기서 오브젝틀를 찾아야함?
    }
    public void  LoadStageData(StageData stageData)
    {
        RegisterSpawners();
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
         // scene 에 소환된 오브젝트 남아있는거 다파괴 
    }
    public void RegisterSpawners()
    {
        Managers.Resource.Instantiate("Player", null, (obj) =>
        {
            PlayerStatus status = obj.GetComponent<PlayerStatus>();
            status.SetInfo();
        });
    }
    public void RegisterPuzzles()
    {
        _leftPuzzles++;
    }

    public void UnRegisterSpawners()
    {
        _leftPuzzles--;
        if (_leftPuzzles <= 0)
        {
            OnAllPuzzlesCleared();
        }
    }
    public void UnRegisterPuzzles()
    {

    }
    private void OnAllPuzzlesCleared()
    {
        Debug.Log("모든 퍼즐 클리어됨, 문을 여시오");

    }
       
        
}
