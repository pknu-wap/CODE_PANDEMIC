using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

public class UI_ItemReward :UI_Base
{
    private void OnEnable()
    {
        Managers.Event.Subscribe("ItemReward", OnItemReward);
    }
    private void OnDisable()
    {
        Managers.Event.Unsubscribe("ItemReward", OnItemReward);
    }
    private void OnItemReward(object obj)
    {
       if(obj is ItemData data)
       {
            Managers.Resource.Instantiate("UI_ItemSlide", transform, (obj) =>
            {
                RectTransform imageTransform=obj.GetComponent<RectTransform>();
                imageTransform.pivot = new Vector2(1, 0.5f);
                UI_ItemSlide slide = obj.GetComponent<UI_ItemSlide>();

                slide.Init();
                Managers.Resource.LoadAsync<Sprite>(data.Sprite, (obj) =>
                {
                    slide.SettingImage(obj, data.Name);
                });
            });

       }
    }

}
