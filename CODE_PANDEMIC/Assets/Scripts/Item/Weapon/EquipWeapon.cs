using Inventory.Model;
using Inventory.Model.Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
   
    private EquippableItem _weapon;
    private QuickSlot _quickSlot;
   
    PlayerControls _weaponInput;
    public GameObject _weaponPrefab;

    //[SerializeField]
    // private List<ItemParameter> _parametersToModify, _itemCurrentState;
    private void Awake()
    {
        _weaponInput=new PlayerControls();  
    }
    private void OnEnable()
    {
        _weaponInput.Player.Equip1.performed -= ctx => EquipQuickSlot(1);
        _weaponInput.Player.Equip2.performed -= ctx => EquipQuickSlot(2);
        _weaponInput.Player.Equip3.performed -= ctx => EquipQuickSlot(3);
        _weaponInput.Player.Equip4.performed -= ctx => EquipQuickSlot(4);

        _weaponInput.Player.Equip1.performed += ctx => EquipQuickSlot(1);
        _weaponInput.Player.Equip2.performed += ctx => EquipQuickSlot(2);
        _weaponInput.Player.Equip3.performed += ctx => EquipQuickSlot(3);
        _weaponInput.Player.Equip4.performed += ctx => EquipQuickSlot(4);

    }
    private void OnDisable()
    {
        _weaponInput.Player.Equip1.performed -= ctx => EquipQuickSlot(1);
        _weaponInput.Player.Equip2.performed -= ctx => EquipQuickSlot(2);
        _weaponInput.Player.Equip3.performed -= ctx => EquipQuickSlot(3);
        _weaponInput.Player.Equip4.performed -= ctx => EquipQuickSlot(4);
    }

    private bool EquipQuickSlot(int v)
    {
        if(_quickSlot.CheckSlot(v))
        return false;

        _quickSlot.UseQuickSlot(v,gameObject);
        return true;
    }

    private void Start()
    {
       _quickSlot = Managers.Game.QuickSlot;
    }
    public void SetWeapon(EquippableItem weaponItem, List<ItemParameter>itemState)
    {
        Debug.Log("Setweapon");
        _weapon = weaponItem;
        
    }
    //private void ModifyParameters()
    //{
    //    foreach(var paramaeter in _parametersToModify)
    //    {
    //        if(_itemCurrentState.Contains(paramaeter))
    //        {
    //            int index =_itemCurrentState.IndexOf(paramaeter);
    //            float newValue = _itemCurrentState[index].value + paramaeter.value;
    //            _itemCurrentState[index] = new ItemParameter
    //            {
    //               parameterName = paramaeter.parameterName,
    //                value = newValue
    //            };
    //        }
    //    }
    //}
}
