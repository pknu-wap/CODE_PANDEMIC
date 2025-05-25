using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    StageData _stageData;
    Dictionary<int, PZ_Main_Block> LinkedBlocks;
    [Header("Light")]
    [SerializeField]
    LightController _light;

    [Header("CameraSetting")]
    [SerializeField]
    private  PolygonCollider2D _cameraLimit;
    [SerializeField]
    private Camera _mapCamera;

    [Header("Transform")]
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
        CreateInteracts();
    }
    private void Start()
    {   
        if(_mapCamera!=null)
        _mapCamera.gameObject.SetActive(false);
        
    }
    public void OnEnable()
    {
        Managers.Event.Subscribe("MainPuzzleClear",OnPuzzleCleared);
        Managers.Event.Subscribe("OnMapCamera", OnMapCamera);
        Managers.Event.Subscribe("OffMapCamera", OffMapCamera);

    }
    public void OnDisable()
    {
        Managers.Event.Unsubscribe("MainPuzzleClear", OnPuzzleCleared);
        Managers.Event.Unsubscribe("OnMapCamera", OnMapCamera);
        Managers.Event.Unsubscribe("OffMapCamera", OffMapCamera);

    }
    public void BossSpawner()
    {

    }
    public void CreateSpawners()
    {
        if (_stageData.BossTemplateID == 0) CreateNormalMapSpawner();
        else CreateBossMapSpawner();
    }   
    private  void CreateNormalMapSpawner()
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
    public void CreateBossMapSpawner()
    {
        List<SpawnerInfoData> spawners = _stageData.Spawners;
        for (int i = 0; i < spawners.Count; i++)
        {
            SpawnerInfoData spawnerData = spawners[i];
            Managers.Resource.Instantiate(spawnerData.Name, _spawnerParent, (obj) =>
            {
                obj.transform.localPosition = new Vector3(spawnerData.Pos.x, spawnerData.Pos.y, 0);
                BossSpawner bossSpawner = obj.GetComponent<BossSpawner>();
                if (bossSpawner != null)
                {
                    bossSpawner.SetInfo(spawnerData.ID);
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
                PZ_Puzzle_Item puzzleItem = obj.GetComponent<PZ_Puzzle_Item>();
                PZ_Puzzle_Base puzzle=  obj.GetComponent<PZ_Puzzle_Base>();  
                if (data.IsMain)
                {
                    if (data.LinkedBlock.Prefab!=null)
                    CreateBlock(data.ID,data.LinkedBlock);
                }

                if (puzzleItem != null)
                {
                    puzzleItem.SetInfo(data);
                    puzzleItem.SettingPuzzle();
                }
                else if (puzzle != null)
                {
                    puzzle.SetInfo(data); //원래 이렇게 하면 안되긴하는데 퍼즐 코드가 너무어지러워서 임시 
                    if (obj.GetComponent<PZ_Generator>() != null)
                    {
                        Color color = new Color(0.02f, 0.02f, 0.02f, 0);
                        _light?.SettingLight(color);
                    }

                }

            });
        }
    }
    private void CreateInteracts()
    {
        List<int> interacts = _stageData.InteractableObjects;
        Debug.Log(Managers.Data.Interacts);
        for (int i = 0; i < interacts.Count; i++)
        {
            if (!Managers.Data.Interacts.TryGetValue(interacts[i],out InteractObjectData data))
            {
                Debug.LogError($"Interact ID :  {interacts[i]} not found in data.");
                continue;
            }
            if (Managers.Game.InteractObjects.Contains(data.ID)) continue;
            Managers.Resource.Instantiate(data.Prefab, _puzzlesParent, (obj) =>
            {
                obj.transform.position = data.Pos;
                obj.GetComponent<PZ_Interact_Spawn>()?.SetInfo(data);
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
            //Destroy(LinkedBlocks[id].gameObject);

            LinkedBlocks[id].Disappear();
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
    private void OnMapCamera(object obj)
    {
        _mapCamera.gameObject.SetActive(true);
    }
    private void OffMapCamera(object  obj)
    {
        _mapCamera.gameObject.SetActive(false);
    }

}
