using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlot : MonoBehaviour
{
    private Dictionary<int, EquipItem> _equipItems = new();

    public void RegisterEquipSlot(EquipItem equipItem)
    {
        if (equipItem == null) return;

        int slotIndex = 0;// EquipSlotIndex.GetSlotIndex(equipItem.ArmorStats.Type);

        if (_equipItems.ContainsKey(slotIndex))
        {
            SwapSlotItem(equipItem);
            return;
        }

        _equipItems[slotIndex] = equipItem;
        NotifySlotUpdate(slotIndex, equipItem);
    }

    public void SwapSlotItem(EquipItem newItem)
    {
        int slotIndex = 0;//EquipSlotIndex.GetSlotIndex(newItem.ArmorStats.Type);

        if (_equipItems.TryGetValue(slotIndex, out var oldItem))
        {
            Managers.Game.Inventory.AddItem(oldItem, 1);
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
      //  Managers.Event.InvokeEvent("OnEquipSlotUpdated", new EquipSlotUpdateData(slotIndex, item));
    }

}
