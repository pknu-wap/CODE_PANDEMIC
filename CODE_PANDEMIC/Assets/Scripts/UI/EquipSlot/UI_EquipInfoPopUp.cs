using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

public class UI_EquipInfoPopUp :UI_PopUp
{
    enum Images
    {
        ItemImage
    }
    enum Texts
    {
        Title,
        Description
    }
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindImage(typeof(Images));  
        BindText(typeof(Texts));
        return true;
    }
    public void SetData(ItemData data,Sprite sprite)
    {
        if(data!=null)
        {
            GetImage((int)Images.ItemImage).sprite = sprite;
            GetText((int)Texts.Title).text = data.Name;
            GetText((int)Texts.Description).text = data.Description;    
        }
    }
}
