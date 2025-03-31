using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    [SerializeField]
    private EquippableItem _weapon;
    [SerializeField]
    private InventoryData _inventoryData;
    [SerializeField]
    private List<ItemParameter> _parametersToModify, _itemCurrentState;

    public void SetWeapon(EquippableItem weaponItemSO, List<ItemParameter>itemState)
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
                   parameterName = paramaeter.parameterName,
                    value = newValue
                };
            }
        }
    }
}
