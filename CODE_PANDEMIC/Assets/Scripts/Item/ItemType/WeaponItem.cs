
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory.Model
{
    public class WeaponItem : ItemData, IDestroyableItem, IItemAction
    {
        public Define.ActionType ActionType => Define.ActionType.QuickSlot;

        public bool PerformAction(GameObject obj, List<ItemParameter> itemState)
        { 
            EquipWeapon weaponSystem =obj.GetComponent<EquipWeapon>();
            Debug.Log("weapon");
            if (weaponSystem!=null)
            {
                weaponSystem.SetWeapon(this, itemState == null ? Parameters : itemState);
                return true;
               
            }
            return false;
        }

    }
}