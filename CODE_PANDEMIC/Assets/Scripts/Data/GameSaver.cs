using Inventory.Model.Inventory.Model;
using Inventory.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using static Define;

#region GameSaver
public class GameSaver
{
    private static string SavePath = Application.persistentDataPath + "/SaveData.json";
   
    public void SaveGame(GameData data)
    {
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
                    item = ItemFactoryManager.CreateItem(item.Type, item);
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
    public List<int> InteractObjectIDS = new();
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
[Serializable]
public class EquipSlotData
{
    public int slotIndex; 
    public int ItemID;
}

[Serializable]
public class EquipSaveData
{
    public List<EquipSlotData> EquipSlots = new();
}

public class EquipSaver
{
    private EquipSlot _equipSlot;
    private static string SavePath => Application.persistentDataPath + "/equip.json";

    public EquipSaver(EquipSlot equipSlot)
    {
        _equipSlot = equipSlot;
    }

    public void SaveEquip()
    {
        var saveData = new EquipSaveData();

        foreach (var pair in _equipSlot.GetEquippedItems())
        {
            saveData.EquipSlots.Add(new EquipSlotData
            {
                slotIndex = pair.Key,
                ItemID = pair.Value.TemplateID,
            });
        }

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(SavePath, json);
        Debug.Log("Equip saved.");
    }

    public void LoadEquip()
    {
        if (!File.Exists(SavePath)) return;

        string json = File.ReadAllText(SavePath);
        var saveData = JsonUtility.FromJson<EquipSaveData>(json);

        foreach (var slotData in saveData.EquipSlots)
        {
            
            if (Managers.Data.Items.TryGetValue(slotData.ItemID, out var itemData))
            {
                EquipItem equipItem = ItemFactoryManager.CreateItem(itemData.Type, itemData) as EquipItem;
                if (equipItem != null)
                {
                    _equipSlot.RegisterEquipSlot(equipItem);
                }
            }
        }

        Debug.Log("Equip loaded.");
    }

    public void DeleteData()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}
public class QuickSlotSaver
{
    private QuickSlot _quickSlot;
    private static string SavePath => Application.persistentDataPath + "/quickslot.json";

    public QuickSlotSaver(QuickSlot quickSlot)
    {
        _quickSlot = quickSlot;
    }

    public void SaveQuickSlot()
    {
        QuickSlotItemData[] slotData = new QuickSlotItemData[4];

        for (int i = 1; i <= 4; i++)
        {
            var slotItem = _quickSlot.GetSlotItem(i);
            slotData[i - 1] = new QuickSlotItemData
            {
                ItemID = slotItem?.ItemData.TemplateID ?? -1,
                Quantity = slotItem?.Quantity ?? 0
            };
        }

        string json = JsonUtility.ToJson(new QuickSlotSaveWrapper { Slots = slotData });
        File.WriteAllText(SavePath, json);
        Debug.Log("QuickSlot saved.");
    }

    public void LoadQuickSlot()
    {
        if (!File.Exists(SavePath)) return;

        string json = File.ReadAllText(SavePath);
        var saveWrapper = JsonUtility.FromJson<QuickSlotSaveWrapper>(json);

        
        _quickSlot.ClearAllSlots();

        for (int i = 1; i <= 4; i++)
        {
            var slotData = saveWrapper.Slots[i-1];
            if (slotData != null && slotData.ItemID != -1)
            {
                if (Managers.Data.Items.TryGetValue(slotData.ItemID, out var itemData))
                {
                    ItemData newItem = ItemFactoryManager.CreateItem(itemData.Type, itemData);
                    _quickSlot.RegisterQuickSlot(newItem, slotData.Quantity);
                }
            }
        }
        Debug.Log("QuickSlot loaded.");
    }

    public void DeleteData()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }

    [Serializable]
    private class QuickSlotSaveWrapper
    {
        public QuickSlotItemData[] Slots;
    }

}
[Serializable]
public class QuickSlotItemData
{
    public int ItemID;
    public int Quantity;
}
