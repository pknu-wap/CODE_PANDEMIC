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

    // 퀵슬롯에 아이템 등록
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

    // 슬롯 이미지 업데이트
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

    // 모든 슬롯 초기화
    public void InitializeAllSlots()
    {
        foreach (var slot in _slotItems)
        {
            UpdateSlotImage(slot.Key, slot.Value);
        }
    }

    // 개별 슬롯 업데이트 (아이템 교체 시 사용)
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

    // 슬롯 해제 (아이템 제거)
    public void ClearSlot(int slotIndex)
    {
        if (_slotItems.ContainsKey(slotIndex))
        {
            _slotItems.Remove(slotIndex);
            GetImage(slotIndex).sprite = null;  // 빈 이미지로 초기화
        }
    }

    // 슬롯별 업데이트 메서드
    public void UpdateShortWeapon(ItemData weapon) => RegisterQuickSlot((int)Images.ShortWeaponImage, weapon);
    public void UpdatePistolWeapon(ItemData weapon) => RegisterQuickSlot((int)Images.PistolWeaponImage, weapon);
    public void UpdateRangeWeapon(ItemData weapon) => RegisterQuickSlot((int)Images.RangeWeaponImage, weapon);
    public void UpdatePortionWeapon(ItemData potion) => RegisterQuickSlot((int)Images.PortionImage, potion);

    // 퀵슬롯 사용 처리 (키보드 입력에 따라 사용)
    public void UseQuickSlot(int slotIndex)
    {
        if (_slotItems.TryGetValue(slotIndex, out ItemData itemData))
        {
            Debug.Log($"Using item from slot {slotIndex}: {itemData.Name}");
            // 아이템 사용 로직
            if (itemData is IItemAction actionItem)
            {
                bool success = actionItem.PerformAction(gameObject, itemData.Parameters);
                if (success)
                {
                    Debug.Log($"{itemData.Name} 사용 완료.");
                }
                else
                {
                    Debug.LogWarning($"{itemData.Name} 사용 실패.");
                }
            }
        }
        else
        {
            Debug.LogWarning($"No item registered in slot {slotIndex}."); 
        }
    }
}
