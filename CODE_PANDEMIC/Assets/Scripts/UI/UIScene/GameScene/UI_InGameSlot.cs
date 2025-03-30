using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_InGameSlot : UI_Base
{
    enum Images
    {
        ShortWeaponImage,
        PistolWeaponImage,
        RangeWeaponImage,
        PortionImage

    }

    public override bool Init()
    {
        if (base.Init() == false) return false;

        BindImage(typeof(Images));
        return true;
    }
     
    public void UpdateShortWeapon(Sprite sprite)
    {
        GetImage((int)Images.ShortWeaponImage).sprite = sprite;
    }
    public void UpdatePistolWepaon(Sprite sprite)
    {
        GetImage((int)Images.PistolWeaponImage).sprite = sprite;
    }
    public void UpdateRangeWepaon(Sprite sprite)
    {
        GetImage((int)Images.RangeWeaponImage).sprite = sprite;
    }
    public void UpdatePortionWeapon(Sprite sprite)
    {
        GetImage((int)Images.PortionImage).sprite = sprite;
    }
    //전체 한번 이미지 가져오는 거 필요함  
}
