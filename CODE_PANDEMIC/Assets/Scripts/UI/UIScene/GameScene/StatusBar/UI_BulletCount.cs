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
    }
    enum Texts
    {
        CountText
    }
    [SerializeField]
    TextMeshProUGUI _text;
    [SerializeField]

    Image _bulletImage;

    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        _text = GetText((int)Texts.CountText);
        _bulletImage=GetImage((int)Images.BulletImage); 

        ResetInfo();
        return true;
    }
    private void OnEnable()
    {
        Managers.Event.Subscribe("ShortWeaponEquipped", OnShortWeaponEquipped);
        Managers.Event.Subscribe("GunWeaponEquipped", OnGunWeaponEquipped);
        Managers.Event.Subscribe("BulletUpdated", OnBulletUpdated); 
        Managers.Event.Subscribe("EquipDisable", OnEquipDisable);
    }

   

    private void OnDisable()
    {
        Managers.Event.Unsubscribe("ShortWeaponEquipped", OnShortWeaponEquipped);
        Managers.Event.Unsubscribe("GunWeaponEquipped", OnGunWeaponEquipped);
        Managers.Event.Unsubscribe("BulletUpdated", OnBulletUpdated);
        Managers.Event.Subscribe("EquipDisable", OnEquipDisable);
    }
    private void ResetInfo()
    {
        _bulletImage.gameObject.SetActive(false);
        _text.text = " ";
    }
    private void OnShortWeaponEquipped(object obj)
    {
        ResetInfo();
    }

    private void OnGunWeaponEquipped(object obj)
    {
        
        _bulletImage.gameObject?.SetActive(true);
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
    private void OnEquipDisable(object obj)
    {
        ResetInfo() ;   
    }

}
