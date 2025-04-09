
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
        { //playerÀÇ weapon socket Àü´Þ 
           
            EquipWeapon weaponSystem =obj.GetComponent<EquipWeapon>();
            if(weaponSystem!=null)
            {
                Debug.Log("EquipAction");
                weaponSystem.SetWeapon(this, itemState == null ? Parameters : itemState);
                return true;
            }
            return false;
        }

    }
}