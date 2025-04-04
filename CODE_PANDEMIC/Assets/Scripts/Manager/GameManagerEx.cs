using Inventory.Model;
using Inventory.Model.Inventory.Model;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GameData
{
    public int Chapter = 1;
    public int Stage = 1;

    public int ShortWeaponID;
    public int PistolID;
    public int RangeWeaponID; //key slot data ÀúÀæ¤· 

    public int MasterVolume;
    public int BgmVolume; //volume ÀúÀå 
    public int EffectVolume;

    public Resolution SaveResolution;
    public bool IsFullScreen;

    public static string FilePath => Application.persistentDataPath + "/SaveData.json";
}


public class GameManagerEx 
{
    private GameData _gameData = new GameData();
    private bool _isPaused;
    private InventoryData _inventoryData;
    private InventorySaver _inventorySaver;

    public GameData SaveData
    {
        get => _gameData;
        set => _gameData = value;
    }
    public InventoryData Inventory
    {
        get=>_inventoryData;
        private set => _inventoryData = value;  
    }

    public int Chapter //1chapter 1stage = º´¿ø 3Ãþ 
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
        _inventorySaver.LoadInventory();
        LoadGame();
    }

    public void SetResolutionScreen(Resolution res)
    {
        Screen.SetResolution(res.width, res.height, SaveData.IsFullScreen);
        SaveData.SaveResolution = res;
        Debug.Log($"Resolution set: {res.width}x{res.height}");
    }

    public void SetFullScreenMode(bool value)
    {
        Screen.fullScreen = value;
        SaveData.IsFullScreen = value;
        Debug.Log($"Fullscreen mode: {value}");
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

    public void SaveGame()
    {
        string jsonStr = JsonUtility.ToJson(_gameData);

        File.WriteAllText(GameData.FilePath, jsonStr);

        _inventorySaver.SaveInventory();

        Debug.Log("Game saved.");
    }

    public bool LoadGame()
    {
        if (!File.Exists(GameData.FilePath)) return false;

        string fileStr = File.ReadAllText(GameData.FilePath);
        GameData data = JsonUtility.FromJson<GameData>(fileStr);
        if (data != null) _gameData = data;
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
