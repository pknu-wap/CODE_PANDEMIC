using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    [SerializeField]
    private EquippableItemSO _weapon;
    [SerializeField]
    private InventoryData _inventoryData;
    [SerializeField]
    private List<ItemParameter> _parametersToModify, _itemCurrentState;

    public void SetWeapon(EquippableItemSO weaponItemSO, List<ItemParameter>itemState)
    {
        Debug.Log("Setweapon");
        if(_weapon!=null)
        {
            _inventoryData.AddItem(_weapon, 1, _itemCurrentState);
        }
        _weapon = weaponItemSO;
        _itemCurrentState = new List<ItemParameter>(itemState);
        ModifyParameters();
    }
    private void ModifyParameters()
    {
        foreach(var paramaeter in _parametersToModify)
        {
            if(_itemCurrentState.Contains(paramaeter))
            {
                int index =_itemCurrentState.IndexOf(paramaeter);
                float newValue = _itemCurrentState[index].value + paramaeter.value;
                _itemCurrentState[index] = new ItemParameter
                {
                    itemParameter = paramaeter.itemParameter,
                    value = newValue
                };
            }
        }
    }
}
