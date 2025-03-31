using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Inventory.Model;
using static Define;
using Unity.VisualScripting;

[Serializable]
public class GameData
{
    public int Chapter=1;
    public int Stage=1;

    public int ShortWeaponID;
    public int PistolID;
    public int RangeWeaponID;

    public int MasterVolume;
    public int BgmVolume;
    public int EffectVolume;

    public Resolution SaveResolution;
    public bool IsFullScreen;
}
public class GameManagerEx : MonoBehaviour
{
    GameData _gameData = new GameData();
    private bool _isPaused;
    private InventoryData _inventoryData;
    private InventorySaver _inventorySaver; // InventorySaver 추가
    public GameData SaveData
    {
        get { return _gameData; }
        set { _gameData = value; }
    }
    public InventoryData Inventory
    {
        get { return _inventoryData; }
    }
    public void Init()
    {
        _isPaused = false;
        _path = Application.persistentDataPath + "/SaveData.json";
        _inventoryData= new InventoryData();
        _inventorySaver = new InventorySaver(_inventoryData);
        _inventorySaver.LoadInventory();
       
    }
    public void SetResolutionScreen(Resolution res)
    {
        Screen.SetResolution(res.width, res.height, Screen.fullScreen = SaveData.IsFullScreen);
        Debug.Log($"{res.width}x{res.height}");
        SaveData.SaveResolution = res;
    }
    public void SetFullScreenMode(bool value)
    {
        Screen.fullScreen = value;
        SaveData.IsFullScreen = value;
    }
    #region SaveLoad
    public bool IsLoaded = false;
    string _path;
    public void PauseGame()
    {
        if (!_isPaused)
        {
            _isPaused = true;
            Time.timeScale = 0;
        }
    }
    public void ResumeGame()
    {
        if (_isPaused)
        {
            _isPaused = false;
            Time.timeScale = 1;
        }
    }
    public void SaveGame()
    {
        string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
        File.WriteAllText(_path, jsonStr);
        _inventorySaver.SaveInventory(); // 인벤토리 저장
    }
    public bool LoadGame()
    {
        if (File.Exists(_path) == false)
            return false;

        string fileStr = File.ReadAllText(_path);
        GameData data = JsonUtility.FromJson<GameData>(fileStr);
        if (data != null)
            Managers.Game.SaveData = data;

        IsLoaded = true;
        return true;
    }
    #endregion

    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}

public class InventorySaver
{
    private InventoryData _inventoryData;
    private string _savePath;

    public InventorySaver(Inventory.Model.InventoryData inventoryData)
    {
        _inventoryData = inventoryData;
        _savePath = Application.persistentDataPath + "/inventory.json";
    }

    public void SaveInventory()
    {
        InventorySaveData saveData = new InventorySaveData();
        saveData.InventoryItems = new List<InventoryItemData>();

        foreach (var item in _inventoryData.GetCurrentInventoryState())
        {
            InventoryItemData itemData = new InventoryItemData
            {
                ItemID = item.Value._item.TemplateID,
                Quantity = item.Value._quantity,
                ItemState = item.Value._itemState
            };
            saveData.InventoryItems.Add(itemData);
        }

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(_savePath, json);
    }

    public void LoadInventory()
    {
        
        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

            if (saveData != null && saveData.InventoryItems != null)
            {
                Dictionary<int, Inventory.Model.InventoryItem> loadedItems = new Dictionary<int, Inventory.Model.InventoryItem>();
                foreach (var itemData in saveData.InventoryItems)
                {
                    if (Managers.Data.Items.TryGetValue(itemData.ItemID, out ItemData item))
                    {
                        loadedItems.Add(loadedItems.Count, new Inventory.Model.InventoryItem(item, itemData.Quantity, itemData.ItemState));
                    }
                    else
                    {
                        Debug.LogError($"Item with ID {itemData.ItemID} not found.");
                    }
                }
                _inventoryData.LoadInventoryFromData(loadedItems);
            }
        }
        else
        {
            _inventoryData.Init();
        }
    }
}