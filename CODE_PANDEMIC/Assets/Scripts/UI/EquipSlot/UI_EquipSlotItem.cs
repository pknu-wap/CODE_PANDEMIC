using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipSlotItem :UI_Base, IPointerClickHandler
{
    enum Images
    {
        ItemImage,
        BGItemImage
    }
    Color _bgOriginal;
    
    public override bool Init()
    {
        if (base.Init() == false) return false;
       
        BindImage(typeof(Images));
       _bgOriginal = GetImage((int)Images.BGItemImage).color;
        return true;
    }
  
    public event Action<UI_EquipSlotItem> OnRightMouseButtonClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) OnRightMouseButtonClick?.Invoke(this); 
    }

    public void UpdateSlotItem(ItemData data)
    {
        Managers.Resource.LoadAsync<Sprite>(data.Sprite, callback: (sprite) =>
        {
            if (sprite != null)
            {
                GetImage((int)Images.ItemImage).sprite = sprite;
                Color color = GetImage((int)Images.BGItemImage).color;
                color.a = 0.0f;
                GetImage((int)Images.BGItemImage).color = color;
            }
            else
            {
                Debug.LogWarning($"[UI_InGameSlot] {data.Name} 스프라이트 로딩 실패");
            }
                
        });
    }
    
    public void UpdateRemoveItem()
    {
        GetImage((int)Images.ItemImage).sprite = null;
        GetImage((int)Images.BGItemImage).color = _bgOriginal;
    }
}
