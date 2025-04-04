using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

namespace Inventory.UI
{
    public class UI_InventoryItem : UI_Base, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        enum GameObjects
        {
            BG,
        }

        enum Images
        {   
            ItemImage
        }
        enum Texts
        { 
            ItemCount
        }
        [SerializeField] Image _itemImage;
        [SerializeField] TextMeshProUGUI _text;
        public event Action<UI_InventoryItem> OnItemClicked, OnItemDroppedOn
             , OnItemBeginDrag, OnItemEndDrag, OnRightMouseButtonClick;
        private bool empty = true;      
       
        public void Initialize()
        {
            BindImage(typeof(Images));
            BindText(typeof(Texts));
            _itemImage = GetImage((int)Images.ItemImage);
            _text = GetText((int)Texts.ItemCount);
            ResetData();
            Deselect();
        }
        public void Deselect()
        {
      
        }

        public void ResetData()
        {

            GetImage((int)Images.ItemImage).gameObject.SetActive(false);
        
            empty = true;
        }
        public void SetData(Sprite sprite, int quantity)
        {
            GetImage((int)Images.ItemImage).gameObject.SetActive(true);
            GetImage((int)Images.ItemImage).sprite = sprite;
            GetText((int)Texts.ItemCount).text=quantity + "";
          ;
            empty = false;
        }
        public void Select()
        {
            // _borderImage.enabled = true;
        }



        public void OnPointerClick(BaseEventData data)
        {
            PointerEventData pointerEventData = data as PointerEventData;
            if (pointerEventData.button == PointerEventData.InputButton.Right) OnRightMouseButtonClick?.Invoke(this); //정보보기 
            else OnItemClicked?.Invoke(this); //단지 선택하는거 
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty) return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {

            if (pointerEventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("right");
                OnRightMouseButtonClick?.Invoke(this); //정보보기
            }
            else OnItemClicked?.Invoke(this); //단지 선택하는거 
        }

        public void OnDrag(PointerEventData eventData)
        {

        }
    }
}