using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffItem : ItemData, IItemAction, IDestroyableItem
{
    public Define.ActionType ActionType => Define.ActionType.Buff;
    
    public bool PerformAction(GameObject character, List<ItemParameter> itemState)
    {
        Managers.Buffs.AddBuff(TemplateID);
        
        return true;
    }

   
}
