using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryDescription : UI_Base
    {
        [SerializeField] Image _itemImage;
        [SerializeField] TMP_Text _title;
        [SerializeField] TMP_Text _description;

        enum GameObjects
        {
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
            if (_itemImage != null)
            {
                _itemImage.sprite = null;
                _itemImage.gameObject.SetActive(false);
            }

            if (_title != null) _title.text = "";
            if (_description != null) _description.text = "";
        }

        public void SetDescription(Sprite sprite, string itemName, string itemDescription)
        {
            if (_itemImage != null)
            {
                _itemImage.gameObject.SetActive(true);
                _itemImage.sprite = sprite;
            }

            if (_title != null) _title.text = itemName;
            if (_description != null) _description.text = itemDescription;
        }
    }
}
