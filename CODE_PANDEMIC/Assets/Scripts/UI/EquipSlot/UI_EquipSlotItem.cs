using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipSlotItem : UI_Base, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    enum Images
    {
        ItemImage,
        BGItemImage
    }
    ItemData _data;
    UI_EquipInfoPopUp _toolTip;
    bool _init = false;
    Color _bgOriginal;
    [SerializeField] Sprite _emptySprite;
    public override bool Init()
    {
        if (_init) return true;
        if (base.Init() == false) return false;
       
        BindImage(typeof(Images));
       
       _bgOriginal = GetImage((int)Images.BGItemImage).color;
        _init = true;
        return true;
    }
   

    public void UpdateSlotItem(ItemData data)
    {
        _data = data;
        Managers.Resource.LoadAsync<Sprite>(data.Sprite, callback: (sprite) =>
        {
          GetImage((int)Images.ItemImage).sprite = sprite;
          Color color = GetImage((int)Images.BGItemImage).color;
          color.a = 0.0f;
          GetImage((int)Images.BGItemImage).color = color;
          
        
        });
    }
    private void CloseToolTip()
    {
        if (_toolTip != null)
        {
            Managers.UI.ClosePopupUI(_toolTip);
            _toolTip = null;
        }
    }
    public void UpdateRemoveItem()
    {
        CloseToolTip();
        GetImage((int)Images.ItemImage).sprite = _emptySprite;
        GetImage((int)Images.BGItemImage).color = _bgOriginal;
        _data = null;
    }

    #region EventHandler
    public event Action<UI_EquipSlotItem> OnRightMouseButtonClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) OnRightMouseButtonClick?.Invoke(this);
        Debug.Log("RightClick");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_data != null)
        {
            Managers.UI.ShowPopupUI<UI_EquipInfoPopUp>("UI_EquipToolTipPopUp", transform, (obj) =>
            {
                obj.transform.localPosition = new Vector3(0,100,0);
                _toolTip = obj;
                obj.Init();
                obj.SetData(_data,GetImage((int)Images.ItemImage).sprite);
            });
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        CloseToolTip();
    }
    #endregion
}

