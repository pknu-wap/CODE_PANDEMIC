using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipPopUp :  UI_Base
{

    enum GameObjects
    { 
         HeadSlot,
         ArmorSlot,
         ShoesSlot,
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

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        InitializeUI();
        UpdateAllSlot();
        return true;
    }
    private void InitializeUI()
    {
        for(int i =0; i<Define.ArmorCount; i++)
        {
            UI_EquipSlotItem slotItem = GetObject(i).GetComponent<UI_EquipSlotItem>();
            slotItem.Init();
            int index = i;  // 클로저 캡처 방지용
            slotItem.OnRightMouseButtonClick += (clickedSlot) => OnSlotRightClicked(index);
        }
    }

    private void OnSlotRightClicked(int index)
    {
        Debug.Log(index);
        Managers.Game.EquipSlot.UnEquipItem(index);
    }

    private void OnEnable()
    {
        Managers.Event.Subscribe("OnEquipSlotUpdated", OnEquipSlotUpdated);
        Managers.Event.Subscribe("StatUpdated", OnStatUpdated);
    }

    private void OnDisable()
    {
        Managers.Event.Unsubscribe("OnEquipSlotUpdated", OnEquipSlotUpdated);
        Managers.Event.Unsubscribe("StatUpdated", OnStatUpdated);
    }
    private void OnEquipSlotUpdated(object obj)
    {
        if (obj is EquipSlotUpdateData data)
        {
            UpdateSlot(data.SlotIndex, data.Item);
            UpdateText();
        }
        else
        {
            Debug.LogWarning("OnEquipSlotUpdated: 데이터 타입 불일치");
        }
            
    }

     private void UpdateAllSlot()
    {
        for (int i = 0; i < Define.ArmorCount; i++) 
        {
            EquipItem item = Managers.Game.EquipSlot.GetEquipItem(i);
            UpdateSlot(i, item);
        }

        UpdateText();
    }

    public void UpdateSlot(int armorIndex, ItemData data)
    {
        int index = armorIndex ;
        
        UpdateSlotImage(index, data);

    }
    public void UpdateSlotImage(int armorIndex,ItemData data)
    {
        Debug.Log(armorIndex);
        UI_EquipSlotItem targetObject = GetObject(armorIndex).GetComponent<UI_EquipSlotItem>();
        if (targetObject == null) return;

        if (data == null)
            targetObject.UpdateRemoveItem();
        else
            targetObject.UpdateSlotItem(data);
    }
    private void OnStatUpdated(object obj)
    {
        UpdateText();
    }

    public void UpdateText()
    {
        PlayerStat playerStat= Managers.Game.PlayerStat;
        GetText((int)Texts.HpText).text = $"{(int)playerStat.CurrentHp}/{playerStat.MaxHp}";
        GetText((int)Texts.ArmorText).text = $"{playerStat.Defend}";
        GetText((int)Texts.SpeedText).text = $"{playerStat.BaseSpeed}";
    }
}
