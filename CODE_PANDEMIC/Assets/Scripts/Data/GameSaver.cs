using Inventory.Model.Inventory.Model;
using Inventory.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;

#region GameSaver
public class GameSaver
{
    private static string SavePath = Application.persistentDataPath + "/SaveData.json";
    private QuickSlot _quickSlot;

    public GameSaver(QuickSlot quickSlot)
    {
        _quickSlot = quickSlot;
    }

    public void SaveGame(GameData data)
    {
        
          for (int i = 1; i <= 4; i++)
        {
            var slotItem = _quickSlot.GetSlotItem(i);
            if (slotItem != null)
            {
                data.QuickSlotItems[i - 1] = new QuickSlotItemData
                {
                    ItemID = slotItem.ItemData.TemplateID,
                    Quantity = slotItem.Quantity
                };
            }
            else
            {
                data.QuickSlotItems[i - 1] = new QuickSlotItemData
                {
                    ItemID = -1,
                    Quantity = 0
                };
            }
        }

        string jsonStr = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, jsonStr);
        Debug.Log("Game saved.");
    }

    public void LoadGame(ref GameData data)
    {
        if (!File.Exists(SavePath)) return;

        string fileStr = File.ReadAllText(SavePath);
        GameData loadedData = JsonUtility.FromJson<GameData>(fileStr);
        if (loadedData != null)
        {
            data = loadedData;

            // QuickSlot 로드 로직을 GameSaver로 이동
            for (int i = 1; i <= 4; i++)
            {
                var slotData = loadedData.QuickSlotItems[i - 1];
                if (slotData != null && slotData.ItemID != -1)
                {
                    if (Managers.Data.Items.TryGetValue(slotData.ItemID, out var itemData))
                    {
                        ItemData newItem = ItemFactoryManager.CreateItem(itemData.Type, itemData);
                        _quickSlot.RegisterQuickSlot(newItem, slotData.Quantity);
                    }
                }
            }
        }
        Debug.Log("Game loaded.");
    }

    public bool HasFile()
    {
        return File.Exists(SavePath);
    }

    public void DeleteData()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save data deleted.");
        }
        else
        {
            Debug.Log("No save data to delete.");
        }
    }
}
#endregion

#region InventorySaver
public class InventorySaver:MonoBehaviour
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
            Debug.Log(item);
            saveData.InventoryItems.Add(new InventoryItemData
            {
                ItemID = item.Value.Item.TemplateID,
                Quantity = item.Value.Quantity,
                ItemState = item.Value.ItemState
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
        Debug.Log(File.ReadAllText(SavePath));
        string json = File.ReadAllText(SavePath);
        var saveData = JsonUtility.FromJson<InventorySaveData>(json);

        if (saveData?.InventoryItems != null)
        {
            var loadedItems = new Dictionary<int, InventoryItem>();
            foreach (var itemData in saveData.InventoryItems)
            {
               
                if (Managers.Data.Items.TryGetValue(itemData.ItemID, out ItemData item))
                {
                    item = ItemFactoryManager.CreateItem(item.Type,item);
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
    public void DeleteData()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save data deleted.");
        }
        else
        {
            Debug.Log("No save data to delete.");
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
    public void DeleteData()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save data deleted.");
        }
        else
        {
            Debug.Log("No save data to delete.");
        }
    }
}