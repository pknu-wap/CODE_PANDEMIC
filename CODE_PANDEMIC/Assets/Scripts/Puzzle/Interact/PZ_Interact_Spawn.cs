using Inventory.Model;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PZ_Interact_Spawn : PZ_Interact_Base
{
    protected InteractObjectData _interactData;
    public void SetInfo(InteractObjectData data)
    {
        _interactData = data;
    }
    
    protected int HasKey()
    {
       int key= Managers.Game.Inventory.HasItem(_interactData.KeyID);
        return key;
    }
    protected void RewardItem(int key)
    {
        if (key != -1)
        {
            Managers.Game.Inventory.RemoveItem(key, 1);
        }
        List<RewardData> rewardItemKey = _interactData.Rewards;
        Debug.Log(_interactData.Rewards.Count);
        for (int i = 0; i < rewardItemKey.Count; i++)
        {
            int index = i;
            if (Managers.Data.Items.TryGetValue(rewardItemKey[index].ID, out ItemData item) == true)
            {
                Managers.Game.Inventory.AddItem(item, rewardItemKey[index].Quantity);
                Debug.Log(item.TemplateID);
            }
            else
            {
                
                Debug.Log("No Reward");
            }

        }
    }
}