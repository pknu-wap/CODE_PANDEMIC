using UnityEngine;
using UnityEngine.UI;
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

    enum Texts
    {
        PortionCount
    }

    public override bool Init()
    {
        if (base.Init() == false) return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
       
        Managers.Event.Subscribe("OnQuickSlotUpdated", OnQuickSlotUpdated);
        Managers.Game.QuickSlot.InitializeAllSlots();
        return true;
    }
    
    private void OnDisable()
    {
        Managers.Event.Unsubscribe("OnQuickSlotUpdated", OnQuickSlotUpdated);
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
            targetImage.color = new Color(1, 1, 1, 0); // 투명
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
                img.color = new Color(1, 1, 1, 0);
            }
        }
        UpdatePortionCount(0);
    }
}
