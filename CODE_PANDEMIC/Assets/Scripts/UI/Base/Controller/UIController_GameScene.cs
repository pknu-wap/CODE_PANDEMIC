using Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController_GameScene : UIController_Base
{
   
    InventoryController _inventory;
    UI_EquipPopUp _equipPopUp;
    protected override bool Init()
    {
        if (base.Init() == false) return false;

        _inventory = Utils.GetOrAddComponent<InventoryController>(gameObject);

        return true;
    }

    protected override void EnableInput()
    {
        _inputActions.UI.Enable();

        _inputActions.UI.Inventory.performed += OnInventory;
        _inputActions.UI.Pause.performed += OnClickEscape;
        _inputActions.UI.MiniMap.performed += OnClickTab;
        _inputActions.UI.Equip.performed += OnClickEquip;
    }

  

    protected override void DisableInput()
    {
        _inputActions.UI.Inventory.performed -= OnInventory;
        _inputActions.UI.Pause.performed -= OnClickEscape;
        _inputActions.UI.MiniMap.performed -= OnClickTab;
        _inputActions.UI.Equip.performed -= OnClickEquip;

        _inputActions.Disable();
    }
    private void OnInventory(InputAction.CallbackContext ctx)
    {
        if (_inventory != null)
        {
            _inventory.ShowHide(ctx);
        }
    }
    private void OnClickEquip(InputAction.CallbackContext context)
    {
        if (Managers.UI.HasEquipPopUpUI()) Managers.UI.CloseEquipPopUpUI();
        else Managers.UI.ShowEquipPopUpUI();
    }

    private void OnClickEscape(InputAction.CallbackContext context)
    {
        if (Managers.UI.HasPopUpUI())
            Managers.UI.ClosePopupUI();
        else if (Managers.UI.HasEquipPopUpUI())
            Managers.UI.CloseEquipPopUpUI();
        else if(Managers.UI.EnlargedMiniMapUI!=null)
            Managers.UI.CloseEnlargedMiniMap();
        else if (Managers.UI.IsOpenInventory())
            Managers.UI.HideInventoryUI();
        else
            Managers.UI.ShowPopupUI<UI_PausePopUp>();

    }
    private void OnClickTab(InputAction.CallbackContext ctx)
    {
        if (Managers.UI.EnlargedMiniMapUI != null)
            Managers.UI.CloseEnlargedMiniMap();
        else
            Managers.UI.OpenEnlargedMiniMap();
    }

   
}
       
         
        
            
        

