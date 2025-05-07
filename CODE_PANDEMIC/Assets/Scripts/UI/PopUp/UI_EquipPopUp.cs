using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipPopUp : UI_PopUp
{
 
    enum Images
    {
        HelmetItemImage,
        ArmorItemImage,
        ShoesItemImage,
        HelmetImage,
        ArmorImage,
        ShoesImage
    }
    enum Texts
    {
        HpText,
        ArmorText,
        SpeedText
    }
   
    public override bool Init()
    {
        if (base.Init() == false) return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));    

        return true;
    }

    public void UpdateSlot(int armorIndex, ItemData data)
    {
        int index = armorIndex - 1;
        
        UpdateSlotImage(index, data);

    }
    public void UpdateSlotImage(int armorIndex,ItemData data)
    {
        if (data == null) return;
        
        Image targetImage=GetImage(armorIndex);
        Image HideImage = GetImage(armorIndex + Define.ArmorIndex);

        Managers.Resource.LoadAsync<Sprite>(data.Sprite, callback: (sprite) =>
        {
            if (sprite != null)
            {
                targetImage.sprite = sprite;
                targetImage.color = Color.white;
                HideImage.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"[UI_InGameSlot] {data.Name} 스프라이트 로딩 실패");
            }
        });
    }
    public void UpdateText()
    {

    }
}
