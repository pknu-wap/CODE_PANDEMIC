using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    StageData _stageData;
    [Header("ParentObject")]
    public Transform _spawnerParent;
    public Transform _puzzlesParent;
    public Transform _ItemsParent;
    public void SetInfo(StageData stageData)
    {
        _stageData = stageData;

        CreateSpawners();
        CreatePuzzles();
        CreateItems();
    }
    public void CreateSpawners()
    {
        List<SpawnerInfoData> spawners = _stageData.Spawners;
        for (int i = 0; i < spawners.Count; i++)
        {
            SpawnerInfoData spawnerData = spawners[i];
            Managers.Resource.Instantiate(spawnerData.Name, _spawnerParent, (obj) =>
            {
                obj.transform.position = new Vector3(spawnerData.Pos.x, spawnerData.Pos.y, 0);
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
    private void CreatePuzzles()
    {
        List<int> puzzles = _stageData.Puzzles;
        for (int i = 0; i < puzzles.Count; i++)
        {
            if (!Managers.Data.Puzzles.TryGetValue(puzzles[i], out PuzzleData data))
            {
                Debug.LogError($"Puzzle ID {puzzles[i]} not found in data.");
                continue;
            }
            if (Managers.Game.ClearPuzzleID.Contains(puzzles[i])) continue;
            Managers.Resource.Instantiate(data.Prefab, _puzzlesParent, (obj) =>
            {
                obj.transform.position = data.Pos;
                PZ_Puzzle_Item item = obj.GetComponent<PZ_Puzzle_Item>();
                if (item != null)
                {
                    item.SetInfo(data);
                    item.SettingPuzzle();
                }

            });
        }
    }
    private void CreateItems()
    { 
    }
}
