using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Inventory.Model
{
    public class EquipItem : ItemData, IDestroyableItem, IItemAction
    {
        public Define.ActionType ActionType => Define.ActionType.Equip;

       
        public bool PerformAction(GameObject character, List<ItemParameter> itemState)
        {
            Managers.Game.EquipSlot.RegisterEquipSlot(this);
            return true;
        }
    }
}