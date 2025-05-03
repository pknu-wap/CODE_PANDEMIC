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
        _inputActions.UI.MiniMap.performed += OnClickTab;
    }

    private void OnDisable()
    {
     
            _inputActions.UI.Inventory.performed -= OnInventory;
            _inputActions.UI.Pause.performed -= OnClickEscape;
             _inputActions.UI.MiniMap.performed -= OnClickTab;   
            _inputActions.Disable();
           
    }

    private void OnInventory(InputAction.CallbackContext ctx)
    {
        if (_inventory != null&&Managers.Scene.CurrentSceneType==Define.SceneType.GameScene)
        {
            _inventory.ShowHide(ctx);
        }
    }


    private void OnClickEscape(InputAction.CallbackContext context)
    {
        if (Managers.UI.HasPopUpUI())
            Managers.UI.ClosePopupUI();
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
