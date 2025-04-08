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
    public void SpawnPlayer()
    { 
        //�� �ε��Ǹ� �ű⼭ ������Ʋ�� ã�ƾ���?
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
         // scene �� ��ȯ�� ������Ʈ �����ִ°� ���ı� 
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
        Debug.Log("��� ���� Ŭ�����, ���� ���ÿ�");

    }
       
        
}
