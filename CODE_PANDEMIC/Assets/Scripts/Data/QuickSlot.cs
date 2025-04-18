using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot
{
    private Dictionary<int, QuickSlotItem> _slotItems = new();

    public void RegisterQuickSlot(ItemData itemData, int quantity)
    {
        if (itemData == null || quantity <= 0) return;

        int slotIndex = QuickSlotIndex.GetSlotIndex(itemData.Weapon);

        if (_slotItems.ContainsKey(slotIndex))
        {
            SwapSlotItem(itemData, quantity);
            return;
        }

        var quickSlotItem = new QuickSlotItem(itemData, quantity);
        _slotItems[slotIndex] = quickSlotItem;

        NotifySlotUpdate(slotIndex, quickSlotItem);
    }

    public void SwapSlotItem(ItemData newItem, int quantity)
    {
        if (newItem == null || quantity <= 0) return;

        int slotIndex = QuickSlotIndex.GetSlotIndex(newItem.Weapon);

        if (_slotItems.TryGetValue(slotIndex, out var oldSlotItem))
        {
            Managers.Game.Inventory.AddItem(oldSlotItem.ItemData, oldSlotItem.Quantity);
        }

        _slotItems[slotIndex] = new QuickSlotItem(newItem, quantity);

        NotifySlotUpdate(slotIndex, _slotItems[slotIndex]);
    }
    public bool HasItem(ItemData itemData)
    {
        int slotIndex = QuickSlotIndex.GetSlotIndex(itemData.Weapon);
        if (_slotItems.TryGetValue(slotIndex, out var slotItem))
        {
            return slotItem.ItemData.TemplateID == itemData.TemplateID;
        }
        return false;
    }
    public int AddStackableItem(int amount)
    {
        int index = QuickSlotIndex.GetSlotIndex(Define.WeaponType.None);
        int remain = _slotItems[index].IncreaseQuantity(amount);
        NotifySlotUpdate(index, _slotItems[index]);
        return remain;
    }
    
    public bool CheckSlot(int slotIndex)
    {
        if(!_slotItems.TryGetValue(slotIndex, out var quickSlotItem)) return false;
        return true;
    }
    public void UseQuickSlot(int slotIndex, GameObject user)
    {
        if (!_slotItems.TryGetValue(slotIndex, out var quickSlotItem)) return;

        if (quickSlotItem.ItemData is IItemAction actionItem)
        {
            bool success = actionItem.PerformAction(user, quickSlotItem.ItemData.Parameters);

            if (!success)
            {
                Debug.LogWarning($"[QuickSlot] {quickSlotItem.ItemData.Name} 사용 실패");
                return;
            }

            switch (quickSlotItem.ItemData.Type)
            {
                case Define.ItemType.Edible:
                    quickSlotItem.DecreaseQuantity();
                    if (quickSlotItem.IsEmpty)
                        ClearSlot(slotIndex);
                    else
                        NotifySlotUpdate(slotIndex, quickSlotItem);
                    break;

                case Define.ItemType.Equippable:
                    Debug.Log($"[QuickSlot] {quickSlotItem.ItemData.Name} 장착 완료");
                    break;
                  
            }
        }
    }

    public void ClearSlot(int slotIndex)
    {
        if (_slotItems.ContainsKey(slotIndex))
        {
            _slotItems.Remove(slotIndex);
            NotifySlotUpdate(slotIndex, null);
        }
    }

    public void InitializeAllSlots()
    {
        foreach (var item in _slotItems)
        {
            NotifySlotUpdate(item.Key, item.Value);
        }
    }

    private void NotifySlotUpdate(int slotIndex, QuickSlotItem item)
    {
        Debug.Log("Notify");
        Managers.Event.InvokeEvent("OnQuickSlotUpdated", new QuickSlotUpdateData(slotIndex, item));
    }

    public int GetItemID(int slotIndex)
    {
        return _slotItems[slotIndex].ItemData.TemplateID;
    }
    public QuickSlotItem GetSlotItem(int slotIndex)
    {
        _slotItems.TryGetValue(slotIndex, out var item);
        return item;
    }
}

public class QuickSlotItem
{
    public ItemData ItemData { get; private set; }
    public int Quantity { get; private set; }

    public QuickSlotItem(ItemData itemData, int quantity = 1)
    {
        ItemData = itemData;
        Quantity = quantity;
    }

    public void DecreaseQuantity(int amount = 1)
    {
        Quantity = Mathf.Max(Quantity - amount, 0);
    }
    public int IncreaseQuantity(int amount)
    {
        int remain = 0;
        if(Quantity+amount>ItemData.MaxStackSize)remain=Quantity+amount-ItemData.MaxStackSize;  
        Quantity = Mathf.Min(Quantity+amount, ItemData.MaxStackSize);

        return remain;
    }

    public bool IsEmpty => Quantity <= 0;
}
public class QuickSlotUpdateData
{
    public int SlotIndex { get; }
    public QuickSlotItem QuickSlotItem { get; }

    public QuickSlotUpdateData(int slotIndex, QuickSlotItem item)
    {
        SlotIndex = slotIndex;
        QuickSlotItem = item;
    }
}