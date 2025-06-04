using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BulletCount :UI_Base
{
   enum Images
    {
        BulletImage,
        CloseWeaponImage
    }
    enum Texts
    {
        CountText
    }
   
    TextMeshProUGUI _text;

    [SerializeField]
    Sprite _throwWeaponSprite;
    [SerializeField]
    Sprite _swingWeaponSprite;

    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        _text = GetText((int)Texts.CountText);

        ResetInfo();
        return true;
    }
    private void OnEnable()
    {
        Managers.Event.Subscribe("ShortWeaponEquipped", OnShortWeaponEquipped);
        Managers.Event.Subscribe("ShortWeaponUpdated", OnShortWeaponUpdated);
        Managers.Event.Subscribe("GunWeaponEquipped", OnGunWeaponEquipped);
        Managers.Event.Subscribe("BulletUpdated", OnBulletUpdated); 
        Managers.Event.Subscribe("EquipDisable", OnEquipDisable);
    }

  
    private void OnDisable()
    {
        Managers.Event.Unsubscribe("ShortWeaponEquipped", OnShortWeaponEquipped);
        Managers.Event.Unsubscribe("ShortWeaponUpdated", OnShortWeaponUpdated);
        Managers.Event.Unsubscribe("GunWeaponEquipped", OnGunWeaponEquipped);
        Managers.Event.Unsubscribe("BulletUpdated", OnBulletUpdated);
        Managers.Event.Unsubscribe("EquipDisable", OnEquipDisable);
    }

    private void ResetInfo()
    {
        for (int i = 0; i < Enum.GetValues(typeof(Images)).Length; i++)
        {
            var img = GetImage(i);
            img?.gameObject.SetActive(false);
        }
        _text.text = " ";
    }
   
    private void OnShortWeaponEquipped(object obj)
    {
        ResetInfo();
        GetImage((int)Images.CloseWeaponImage).gameObject.SetActive(true);
        GetImage((int)Images.CloseWeaponImage).sprite = _swingWeaponSprite;
    }

    private void OnGunWeaponEquipped(object obj)
    {
        ResetInfo();
        GetImage((int)Images.BulletImage).gameObject?.SetActive(true);
        if (obj is int count)
        {
            _text.text = count.ToString();
        }
    }

    private void OnBulletUpdated(object obj)
    {
        if(obj is int count)
        {
            _text.text = count.ToString();
        }
    }
    private void OnShortWeaponUpdated(object obj)
    {
        if(obj is int state)
        {
            if(state==0)
            {
                GetImage((int)Images.CloseWeaponImage).sprite = _swingWeaponSprite;
            }
            else
            {
                GetImage((int)Images.CloseWeaponImage).sprite = _throwWeaponSprite;
            }
        }
    }

    private void OnEquipDisable(object obj)
    {
        ResetInfo() ;   
    }

}
