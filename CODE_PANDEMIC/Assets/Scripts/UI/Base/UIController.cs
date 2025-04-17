using Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    PlayerInput _inputActions;
    InventoryController _inventory;
    private void Awake()
    {
        _inputActions = new PlayerInput();
        _inventory = Utils.GetOrAddComponent<InventoryController>(gameObject);
    }
    private void OnEnable()
    {
        _inputActions = new PlayerInput();
        _inputActions.UI.Enable();

        _inputActions.UI.Inventory.performed += OnInventory;
        _inputActions.UI.Pause.performed += OnClickEscape;
    }

    private void OnDisable()
    {
        if (_inputActions != null)
        {
            _inputActions.UI.Inventory.performed -= OnInventory;
            _inputActions.UI.Pause.performed -= OnClickEscape;

            _inputActions.Disable();
            _inputActions.Dispose(); 
            _inputActions = null;
        }
    }

    private void OnInventory(InputAction.CallbackContext ctx)
    {
        if (_inventory != null)
        {
            _inventory.ShowHide(ctx);
        }
    }


    private void OnClickEscape(InputAction.CallbackContext context)
    {
        if (Managers.UI.HasPopUpUI())
            Managers.UI.ClosePopupUI();
        else
        Managers.UI.ShowPopupUI<UI_PausePopUp>();
    }

}
