using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    Dictionary<int, PZ_Main_Block> LinkedBlocks;
    StageData _stageData;
    [SerializeField]
    private  PolygonCollider2D _cameraLimit;
    [Header("ParentObject")]
    public Transform _spawnerParent;
    public Transform _puzzlesParent;
    public Transform _ItemsParent;
    public Transform _blockParent;
    public PolygonCollider2D CameraLimit { get { return _cameraLimit; } private set { _cameraLimit = value; } }
    public void SetInfo(StageData stageData)
    {
        LinkedBlocks = new Dictionary<int, PZ_Main_Block>();
        _stageData = stageData;

        CreateSpawners();
        CreatePuzzles();
        CreateItems();
    }
    public void OnEnable()
    {
        Managers.Event.Subscribe("MainPuzzleClear",OnPuzzleCleared);
    }
    public void OnDisable()
    {
        Managers.Event.Unsubscribe("MainPuzzleClear", OnPuzzleCleared);
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
            if (Managers.Game.ClearPuzzleID.Contains(puzzles[i])) continue; //clear puzzle ignore
            Managers.Resource.Instantiate(data.Prefab, _puzzlesParent, (obj) =>
            {
                obj.transform.position = data.Pos;
                PZ_Puzzle_Item puzzleItem = obj.GetComponent<PZ_Puzzle_Item>();
                PZ_Generator generatorItem = obj.GetComponent<PZ_Generator>();
                if (data.IsMain)
                {
                    CreateBlock(data.ID,data.LinkedBlock);
                }
                if (puzzleItem != null)
                {
                    puzzleItem.SetInfo(data);
                    puzzleItem.SettingPuzzle();
                }
                else if (generatorItem != null)
                {
                    generatorItem.SetInfo(data);
                }

            });
        }
    }
    private void CreateBlock(int id, BlockData blockData)
    {
        Managers.Resource.Instantiate(blockData.Prefab, _blockParent.transform, (obj) =>
        {
            PZ_Main_Block linkedBlock =obj.GetComponent<PZ_Main_Block>();
            linkedBlock.SetInfo(blockData);
            LinkedBlocks.Add(id, linkedBlock);
        });
    }
    private void DestroyLinkedBlock(int id)
    {
        if(LinkedBlocks.ContainsKey(id))
        {
            Destroy(LinkedBlocks[id].gameObject);
            LinkedBlocks.Remove(id);
        }
    }
    private void CreateItems()
    {
        
       List <int> FieldItems = _stageData.FieldItems;
       
        for (int i = 0; i < FieldItems.Count; i++)
        {   
            int dataId = FieldItems[i];
            if (Managers.Game.ObtainedItemIDs.Contains(dataId))
            {
                Debug.Log(FieldItems[i]);
                continue;
            }
            if (Managers.Data.FieldItems.TryGetValue(dataId, out FieldItemData data) == false) continue;    
            Managers.Resource.Instantiate("Item", _ItemsParent, (obj) =>
            {
                Item item=obj.GetComponent<Item>();
                obj.transform.position = data.Pos;
                item.SetInfo(data);
            });
        }
    }
    private void OnPuzzleCleared(object obj)
    {
        var puzzle = obj as PZ_Puzzle_Item;

        if (puzzle != null)
        {
           DestroyLinkedBlock(puzzle.ID);
        }
    }
        
}
