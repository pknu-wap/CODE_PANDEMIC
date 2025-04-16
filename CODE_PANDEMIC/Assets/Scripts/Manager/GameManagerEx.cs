using Inventory.Model;
using Inventory.Model.Inventory.Model;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#region GameData 구조체 정의
[Serializable]
public class QuickSlotItemData
{
    public int ItemID;
    public int Quantity;
}

[Serializable]
public class GameData
{
    public int HighestChapter = 1;
    public int HighestStage = 1;
    public int Chapter =1;
    public int Stage = 1;
    public QuickSlotItemData[] QuickSlotItems = new QuickSlotItemData[4];

    public int MasterVolume;
    public int BgmVolume;
    public int EffectVolume;

    public Resolution SaveResolution;
    public bool IsFullScreen;
    public static string FilePath => Application.persistentDataPath + "/SaveData.json";
}
#endregion

#region GameManagerEx
public class GameManagerEx
{
    private GameData _gameData = new GameData();
    private bool _isPaused;
    private InventoryData _inventoryData;
    private InventorySaver _inventorySaver;
    private QuickSlot _quickSlot;

    private StageProgressData _stageProgressData;
    private StageProgressSaver _stageProgressSaver;

    private HashSet<int> _clearPuzzleID;
    private HashSet<int> _obtainedItemIDs;
    public HashSet<int> ClearPuzzleID => _clearPuzzleID;
    public HashSet<int> ObtainedItemIDs => _obtainedItemIDs;

    private int _prevStage;
    private int _prevChapter;


    public GameData SaveData
    {
        get => _gameData;
        set => _gameData = value;
    }

    public InventoryData Inventory => _inventoryData;
    public QuickSlot QuickSlot => _quickSlot;

    public int LatestChapter
    {
        get => _prevChapter;
        set => _prevChapter = value;
    }
    public int LatestStage
    {
        get => _prevStage;
        set => _prevStage = value;
    }
    public int HighestChapter
    {
        get => _gameData.HighestChapter;
        set => _gameData.HighestChapter = value;
    }
    public int HighestStage
    {
        get => _gameData.HighestStage;
        set => _gameData.HighestStage = value;
    }
    public int Chapter
    {
        get => _gameData.Chapter;
        set => _gameData.Chapter = value;
    }
    public int Stage
    {
        get => _gameData.Stage;
        set => _gameData.Stage = value;
    }

    public void Init()
    {
        _isPaused = false;
        _inventoryData = new InventoryData();
        _inventorySaver = new InventorySaver(_inventoryData);
        _quickSlot = new QuickSlot();
        _inventorySaver.LoadInventory();
        _stageProgressSaver = new StageProgressSaver();
        _stageProgressData = _stageProgressSaver.Load();
        _clearPuzzleID = new HashSet<int>(_stageProgressData.ClearedPuzzleIDs);
        _obtainedItemIDs = new HashSet<int>(_stageProgressData.ObtainedItemIDs);

        _clearPuzzleID = new HashSet<int>(_stageProgressData.ClearedPuzzleIDs);

        LoadGame();
    }

    public void SetResolutionMode(Resolution res)
    {
        Screen.SetResolution(res.width, res.height, SaveData.IsFullScreen);
        SaveData.SaveResolution = res;
       
    }

    public void SetScreenMode(bool value)
    {
        Screen.fullScreen = value;
        SaveData.IsFullScreen = value;
       
    }

    public void PauseGame()
    {
        if (!_isPaused)
        {
            _isPaused = true;
            Time.timeScale = 0;
            Debug.Log("Game paused.");
        }
    }

    public void ResumeGame()
    {
        if (_isPaused)
        {
            _isPaused = false;
            Time.timeScale = 1;
            Debug.Log("Game resumed.");
        }
    }

    public void ClearPuzzle(int id)
    {
        if (_clearPuzzleID.Add(id))
        {
            _stageProgressData.ClearedPuzzleIDs.Add(id);
            _stageProgressSaver.Save(_stageProgressData);
        }
    }

    public void ObtainItem(int mapItemId)
    {
        if (_obtainedItemIDs.Add(mapItemId))
        {
            _stageProgressData.ObtainedItemIDs = new List<int>(_obtainedItemIDs);
            _stageProgressSaver.Save(_stageProgressData);
        }
    }

    public void CompleteStage()
    {
        if (Stage == Define.STAGES_PER_CHAPTER)
        {
            Chapter++;
            Stage = 1;
        }
        else
        {
            Stage++;
        }

        if (Chapter > HighestChapter || (Chapter == HighestChapter && Stage > HighestStage))
        {
            HighestChapter = Chapter;
            HighestStage = Stage;
        }

        SaveGame();
    }

    public void PrevStage()
    {
        if (Chapter == 1 && Stage == 1) return;

        Stage--;
        if (Stage == 0)
        {
            Chapter--;
            Stage = Define.STAGES_PER_CHAPTER;
        }
    }

    public void SaveGame()
    {
        // QuickSlot 저장
        for (int i = 1; i <= 4; i++)
        {
            var slotItem = QuickSlot.GetSlotItem(i);
            if (slotItem != null)
            {
                _gameData.QuickSlotItems[i - 1] = new QuickSlotItemData
                {
                    ItemID = slotItem.ItemData.TemplateID,
                    Quantity = slotItem.Quantity
                };
            }
            else
            {
                _gameData.QuickSlotItems[i - 1] = new QuickSlotItemData
                {
                    ItemID = -1,
                    Quantity = 0
                };
            }
        }

        string jsonStr = JsonUtility.ToJson(_gameData);
        File.WriteAllText(GameData.FilePath, jsonStr);

        _inventorySaver.SaveInventory();
        _stageProgressSaver.Save(_stageProgressData);

        Debug.Log("Game saved.");
    }

    public bool LoadGame()
    {
        if (!File.Exists(GameData.FilePath)) return false;

        string fileStr = File.ReadAllText(GameData.FilePath);
        GameData data = JsonUtility.FromJson<GameData>(fileStr);
        if (data != null)
        {
            _gameData = data;

            for (int i = 1; i <= 4; i++)
            {
                var slotData = data.QuickSlotItems[i - 1];
                if (slotData != null && slotData.ItemID != -1)
                {
                    if (Managers.Data.Items.TryGetValue(slotData.ItemID, out var itemData))
                    {
                        QuickSlot.RegisterQuickSlot(itemData, slotData.Quantity);
                    }
                }
            }
        }

        Debug.Log("Game loaded.");
        return true;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        Debug.Log("Game Quit.");
    }
}

#endregion


#region InventorySaver
public class InventorySaver
{
    private InventoryData _inventoryData;
    private static string SavePath => Application.persistentDataPath + "/inventory.json";

    public InventorySaver(InventoryData inventoryData)
    {
        _inventoryData = inventoryData;
    }

    public void SaveInventory()
    {
        var saveData = new InventorySaveData { InventoryItems = new List<InventoryItemData>() };

        foreach (var item in _inventoryData.GetCurrentInventoryState())
        {
            saveData.InventoryItems.Add(new InventoryItemData
            {
                ItemID = item.Value._item.TemplateID,
                Quantity = item.Value._quantity,
                ItemState = item.Value._itemState
            });
        }

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(SavePath, json);
        Debug.Log("Inventory saved.");
    }

    public void LoadInventory()
    {
        if (!File.Exists(SavePath))
        {
            _inventoryData.Init();
            return;
        }

        string json = File.ReadAllText(SavePath);
        var saveData = JsonUtility.FromJson<InventorySaveData>(json);

        if (saveData?.InventoryItems != null)
        {
            var loadedItems = new Dictionary<int, InventoryItem>();
            foreach (var itemData in saveData.InventoryItems)
            {
                if (Managers.Data.Items.TryGetValue(itemData.ItemID, out var item))
                {
                    loadedItems.Add(loadedItems.Count, new InventoryItem(item, itemData.Quantity, itemData.ItemState));
                    Debug.Log($"Loaded item: {item.Name}, Quantity: {itemData.Quantity}");
                }
                else
                {
                    Debug.LogError($"Item with ID {itemData.ItemID} not found.");
                }
            }
            _inventoryData.LoadInventoryFromData(loadedItems);
            Debug.Log("Inventory loaded.");
        }
        else
        {
            _inventoryData.Init();
        }
    }
}
#endregion
[Serializable]
public class StageProgressData
{
    public List<int> ObtainedItemIDs = new();
    public List<int> ClearedPuzzleIDs = new();
}

public class StageProgressSaver
{
    private static string SavePath => Application.persistentDataPath + "/stageProgress.json";
    private StageProgressData _data = new();

    public void Save(StageProgressData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
    }

    public StageProgressData Load()
    {
        if (!File.Exists(SavePath)) return new StageProgressData();

        string json = File.ReadAllText(SavePath);
     
        return JsonUtility.FromJson<StageProgressData>(json);
    }
}