
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EquippableItemSO : ItemData, IDestroyableItem,IItemAction
    {
        public string ActionName => "Equip";

        public bool PerformAction(GameObject obj, List<ItemParameter> itemState)
        {
           
           AgentWeapon weaponSystem =obj.GetComponent<AgentWeapon>();
            if(weaponSystem!=null)
            {
                Debug.Log("EquipAction");
                weaponSystem.SetWeapon(this, itemState == null ? _parameterList : itemState);
                return true;
            }
            return false;
        }

    }
}