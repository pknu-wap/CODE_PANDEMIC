using Inventory.Model;
using Inventory.Model.Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public class EquipWeapon : MonoBehaviour
{
   
    private EquippableItem _weapon;
    private QuickSlot _quickSlot;
   
    PlayerInput _weaponInput;
    public GameObject _weaponPrefab;

    //[SerializeField]
    // private List<ItemParameter> _parametersToModify, _itemCurrentState;
    private void Awake()
    {
        _weaponInput=new PlayerInput();  
    }
    private void OnEnable()
    {
        _weaponInput.QuickSlot.Equip1.performed += Equip1;
        _weaponInput.QuickSlot.Equip2.performed += Equip2;
        _weaponInput.QuickSlot.Equip3.performed += Equip3;
        _weaponInput.QuickSlot.Equip4.performed += Equip4;

        _weaponInput.Enable();
    }

    private void OnDisable()
    {
        _weaponInput.QuickSlot.Equip1.performed -= Equip1;
        _weaponInput.QuickSlot.Equip2.performed -= Equip2;
        _weaponInput.QuickSlot.Equip3.performed -= Equip3;
        _weaponInput.QuickSlot.Equip4.performed -= Equip4;

        _weaponInput.Disable();
    }

    private bool EquipQuickSlot(int v)
    {
        if(!_quickSlot.CheckSlot(v))
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
      
        _weapon = weaponItem;
        if(Managers.Data.Weapons==null)
        {
            //TODO MAKE WEAPON DATA
            return;
        }
        Managers.Data.Weapons.TryGetValue(weaponItem.TemplateID, out WeaponData data);
        if(data==null)
        {
            Debug.Log("WEAPON"); 
        }
        else
        {
            Managers.Resource.Instantiate(data.WeaponPrefab, gameObject.transform ,(obj) =>
            {
                _weaponPrefab = obj;
            });
        }
        
    }

    #region InputSystem

    private void Equip1(UnityEngine.InputSystem.InputAction.CallbackContext ctx) => EquipQuickSlot(1);
    private void Equip2(UnityEngine.InputSystem.InputAction.CallbackContext ctx) => EquipQuickSlot(2);
    private void Equip3(UnityEngine.InputSystem.InputAction.CallbackContext ctx) => EquipQuickSlot(3);
    private void Equip4(UnityEngine.InputSystem.InputAction.CallbackContext ctx) => EquipQuickSlot(4);

    #endregion
  
}
