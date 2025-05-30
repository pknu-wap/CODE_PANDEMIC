using UnityEngine;
using UnityEngine.UI;
using Inventory.Model;
using System;

public class UI_InGameSlot : UI_Base
{
    enum Images
    {
        ShortWeaponImage,
        PistolWeaponImage,
        RangeWeaponImage,
        PortionImage
    }
    enum GameObjects
    {
        PistolCoolTime,
        RangeCoolTime
    }
    enum Texts
    {
        PortionCount
    }
    UI_CoolTime _pistolCoolTime;
    UI_CoolTime _RangeCoolTime;
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        _pistolCoolTime=GetObject((int)GameObjects.PistolCoolTime).GetComponent<UI_CoolTime>(); 
        _RangeCoolTime=GetObject((int)GameObjects.RangeCoolTime).GetComponent<UI_CoolTime>();       

        Managers.Event.Subscribe("OnQuickSlotUpdated", OnQuickSlotUpdated);
        

        Managers.Game.QuickSlot.InitializeAllSlots();
        return true;
    }
    private void OnEnable()
    {
        Managers.Event.Subscribe("Reload", OnReload);
        Managers.Event.Subscribe("CancelReload", OnCancelCoolTime);
    }

    private void OnCancelCoolTime(object obj)
    {
        _pistolCoolTime.ResetCoolTime();
        _RangeCoolTime.ResetCoolTime();
    }

    private void OnDisable()
    {
        Managers.Event.Unsubscribe("OnQuickSlotUpdated", OnQuickSlotUpdated);
        Managers.Event.Unsubscribe("Reload", OnReload);
    }

    private void OnReload(object obj)
    {
       if(obj is WeaponData data)
        {
           switch(data.Type)
            {
                case Define.WeaponType.PistolWeapon:
                    _pistolCoolTime.StartCoolTime(data.ReloadTime);
                    break;
                case Define.WeaponType.RangeWeapon:
                    _RangeCoolTime.StartCoolTime(data.ReloadTime); 
                    break;
                  

            }
        }
    }

    private void OnQuickSlotUpdated(object data)
    {
        
        if (data is not QuickSlotUpdateData update) return;
        UpdateSlot(update.SlotIndex, update.QuickSlotItem);
    }

    public void UpdateSlot(int slotIndex, QuickSlotItem quickSlotItem)
    {
        int index = slotIndex - 1;
        ItemData itemData = quickSlotItem?.ItemData;
        UpdateSlotImage(index, itemData);

        if ((Images)index == Images.PortionImage)
        {
            int count = quickSlotItem?.Quantity ?? 0;
            UpdatePortionCount(count);
        }
    }

    public void UpdateSlotImage(int slotIndex, ItemData itemData)
    {
        Image targetImage = GetImage(slotIndex);

        if (targetImage == null)
        {
            Debug.LogWarning($"[UI_InGameSlot] 슬롯 {slotIndex}에 이미지 없음");
            return;
        }

        if (itemData == null)
        {
            targetImage.sprite = null;
            targetImage.color = Color.black;
            return;
        }

        Managers.Resource.LoadAsync<Sprite>(itemData.Sprite, callback: (sprite) =>
        {
            if (sprite != null)
            {
                targetImage.sprite = sprite;
                targetImage.color = Color.white;
            }
            else
            {
                Debug.LogWarning($"[UI_InGameSlot] {itemData.Name} 스프라이트 로딩 실패");
            }
        });
    }

    public void UpdatePortionCount(int itemCount)
    {
        var text = GetText((int)Texts.PortionCount);
        text.text = itemCount > 0 ? itemCount.ToString() : "";
    }

    public void ClearAllSlotImages()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(Images)).Length; i++)
        {
            var img = GetImage(i);
            if (img != null)
            {
                img.sprite = null;
                img.color = Color.black;
            }
        }
        UpdatePortionCount(0);
    }
}
