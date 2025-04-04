using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{ //Inventory Descript Classs

    public class UIInventoryDescription : UI_Base
    {
        [SerializeField]
        Image _itemImage;
        [SerializeField]
        TMP_Text _title;
        [SerializeField]
        TMP_Text _description;
        enum GameObjects
        {
            BG,
            ImagePanel,
            DescriptionPanel
            
        }
        enum Images
        {   
            FrameImage,
            ItemImage,
        }

        enum Texts
        { 
            TitleText,
            DescriptionText
        }

    
        public override bool Init()
        {
            if (base.Init() == false) return false;
            BindImage(typeof(Images));
            BindObject(typeof(GameObjects));    
            BindText(typeof(Texts));

            _itemImage = GetImage((int)Images.ItemImage);
            _title = GetText((int)Texts.TitleText);
            _description = GetText((int)Texts.DescriptionText);

            ResetDescription();
            return true;
        }

        public void ResetDescription()
        {
            Debug.Log("Reset");
            _title.text = "";
            _description.text = "";
          
        }
        //각자 아이템 관련 dataloader 캐싱 필요해보임 
        public void SetDescription(Sprite sprite, string itemName, string itemDescription)
        {
            _itemImage.gameObject.SetActive(true);
            _itemImage.sprite = sprite;
            _title.text = itemName;
            _description.text = itemDescription;
          

        }
    }
}