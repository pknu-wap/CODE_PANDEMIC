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
            int index = i;  // Ŭ���� ĸó ������
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
    }

    private void OnDisable()
    {
        Managers.Event.Unsubscribe("OnEquipSlotUpdated", OnEquipSlotUpdated);
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
            Debug.LogWarning("OnEquipSlotUpdated: ������ Ÿ�� ����ġ");
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
    public void UpdateText()
    {

    }
}
