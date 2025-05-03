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
    
    protected void RewardItem()
    {
        List<RewardData> rewardItemKey = _interactData.Rewards;
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