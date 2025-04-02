using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class UI_InGameSlot : UI_Base
{
    enum Images
    {
        ShortWeaponImage,
        PistolWeaponImage,
        RangeWeaponImage,
        PortionImage
    }

    private Dictionary<int, ItemData> _slotItems = new Dictionary<int, ItemData>();

    public override bool Init()
    {
        if (base.Init() == false) return false;

        BindImage(typeof(Images));
        return true;
    }

    // �����Կ� ������ ���
    public void RegisterQuickSlot(int slotIndex, ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogWarning($"Attempted to register a null item in slot {slotIndex}.");
            return;
        }

        _slotItems[slotIndex] = itemData;
        UpdateSlotImage(slotIndex, itemData);
    }

    // ���� �̹��� ������Ʈ
    private void UpdateSlotImage(int slotIndex, ItemData itemData)
    {
        if (itemData == null) return;

        Managers.Resource.LoadAsync<Sprite>(itemData.Sprite, callback: (sprite) =>
        {
            if (sprite != null)
            {
                GetImage(slotIndex).sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"Failed to load sprite for item: {itemData.Name}");
            }
        });
    }

    // ��� ���� �ʱ�ȭ
    public void InitializeAllSlots()
    {
        foreach (var slot in _slotItems)
        {
            UpdateSlotImage(slot.Key, slot.Value);
        }
    }

    // ���� ���� ������Ʈ (������ ��ü �� ���)
    public void UpdateSlot(int slotIndex, ItemData newItem)
    {
        if (_slotItems.ContainsKey(slotIndex))
        {
            _slotItems[slotIndex] = newItem;
            UpdateSlotImage(slotIndex, newItem);
        }
        else
        {
            Debug.LogWarning($"Slot {slotIndex} not found.");
        }
    }

    // ���� ���� (������ ����)
    public void ClearSlot(int slotIndex)
    {
        if (_slotItems.ContainsKey(slotIndex))
        {
            _slotItems.Remove(slotIndex);
            GetImage(slotIndex).sprite = null;  // �� �̹����� �ʱ�ȭ
        }
    }

    // ���Ժ� ������Ʈ �޼���
    public void UpdateShortWeapon(ItemData weapon) => RegisterQuickSlot((int)Images.ShortWeaponImage, weapon);
    public void UpdatePistolWeapon(ItemData weapon) => RegisterQuickSlot((int)Images.PistolWeaponImage, weapon);
    public void UpdateRangeWeapon(ItemData weapon) => RegisterQuickSlot((int)Images.RangeWeaponImage, weapon);
    public void UpdatePortionWeapon(ItemData potion) => RegisterQuickSlot((int)Images.PortionImage, potion);

    // ������ ��� ó�� (Ű���� �Է¿� ���� ���)
    public void UseQuickSlot(int slotIndex)
    {
        if (_slotItems.TryGetValue(slotIndex, out ItemData itemData))
        {
            Debug.Log($"Using item from slot {slotIndex}: {itemData.Name}");
            // ������ ��� ����
            if (itemData is IItemAction actionItem)
            {
                bool success = actionItem.PerformAction(gameObject, itemData.Parameters);
                if (success)
                {
                    Debug.Log($"{itemData.Name} ��� �Ϸ�.");
                }
                else
                {
                    Debug.LogWarning($"{itemData.Name} ��� ����.");
                }
            }
        }
        else
        {
            Debug.LogWarning($"No item registered in slot {slotIndex}."); 
        }
    }
}
