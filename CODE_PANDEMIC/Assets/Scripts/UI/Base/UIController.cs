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
        _inputActions.UI.Enable();

        _inputActions.UI.Inventory.performed -= _inventory.ShowHide;
        _inputActions.UI.Pause.performed -= OnClickEscape;
        _inputActions.UI.Inventory.performed += _inventory.ShowHide;
        _inputActions.UI.Pause.performed += OnClickEscape;

    }


    private void OnClickEscape(InputAction.CallbackContext context)
    {
        if (Managers.UI.HasPopUpUI())
            Managers.UI.ClosePopupUI();
        else
        Managers.UI.ShowPopupUI<UI_PausePopUp>();
    }

}
