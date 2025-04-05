using Inventory.Model;
using Inventory.Model.Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    [SerializeField]
    private EquippableItem _weapon;
    [SerializeField]
    private QuickSlot _quickSlot;
    [SerializeField]
    private List<ItemParameter> _parametersToModify, _itemCurrentState;
    private void Start()
    {
       _quickSlot = Managers.Game.QuickSlot;
    }
    public void SetWeapon(EquippableItem weaponItem, List<ItemParameter>itemState)
    {
        Debug.Log("Setweapon");
        _weapon = weaponItem;
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
