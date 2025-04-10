
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory.Model
{

    public class EquippableItem : ItemData, IDestroyableItem,IItemAction
    {
        public string ActionName => "Equip";

        public bool PerformAction(GameObject obj, List<ItemParameter> itemState)
        { 
           
            EquipWeapon weaponSystem =obj.GetComponent<EquipWeapon>();
            if(weaponSystem!=null)
            {
               
                weaponSystem.SetWeapon(this, itemState == null ? Parameters : itemState);
                return true;
            }
            return false;
        }

    }
}