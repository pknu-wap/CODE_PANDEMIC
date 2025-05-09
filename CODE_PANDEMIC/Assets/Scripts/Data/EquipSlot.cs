using Inventory.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlot : MonoBehaviour
{
    private Dictionary<int, EquipItem> _equipItems = new();

    public void RegisterEquipSlot(EquipItem equipItem)
    {
        if (equipItem == null) return;

        if (Managers.Data.Armors.TryGetValue(equipItem.TemplateID, out ArmorData data) == false) return;

        int slotIndex = EquipSlotIndex.GetSlotIndex(data.Type);
        if (_equipItems.ContainsKey(slotIndex))
        {
            SwapSlotItem(equipItem,slotIndex);
            return;
        }

        _equipItems[slotIndex] = equipItem;
        NotifySlotUpdate(slotIndex, equipItem);
    }

    public void SwapSlotItem(EquipItem newItem,int index)
    {
        int slotIndex = index;

        if (_equipItems.TryGetValue(slotIndex, out var oldItem))
        {
            Managers.Data.Items.TryGetValue(oldItem.TemplateID, out ItemData data);
            Managers.Game.Inventory.AddItem(data, 1);
        }

        _equipItems[slotIndex] = newItem;
        NotifySlotUpdate(slotIndex, newItem);
    }

    public EquipItem GetEquipItem(int slotIndex)
    {
        _equipItems.TryGetValue(slotIndex, out var equipItem);
        return equipItem;
    }

    private void NotifySlotUpdate(int slotIndex, EquipItem item)
    {
       Managers.Event.InvokeEvent("OnEquipSlotUpdated", new EquipSlotUpdateData(slotIndex, item));
    }

    public Dictionary<int, EquipItem> GetEquippedItems()
    {
        return _equipItems;
    }
    public void UnEquipItem(int slotIndex)
    {
        if (_equipItems.TryGetValue(slotIndex, out EquipItem oldItem))
        {
            Managers.Data.Items.TryGetValue(oldItem.TemplateID, out ItemData item);
            Managers.Game.Inventory.AddItem(item, 1);
            _equipItems.Remove(slotIndex);

            NotifySlotUpdate(slotIndex, null);
        }
    }
    public void ClearSlots()
    {
        _equipItems.Clear();
    }
}

public  class EquipSlotUpdateData
{
    private int _slotIndex;
    private EquipItem _item;
    public int SlotIndex => _slotIndex;
    public EquipItem Item => _item;

    public EquipSlotUpdateData(int slotIndex, EquipItem item)
    {
        _slotIndex = slotIndex;
        _item = item;
    }
}