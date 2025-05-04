using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController_TitleScene : UIController_Base
{
    protected override void EnableInput()
    {
        _inputActions.UI.Enable();
        _inputActions.UI.Pause.performed += OnClickEscape;
    }

    protected override void DisableInput()
    {
        _inputActions.UI.Pause.performed -= OnClickEscape;
        _inputActions.Disable();
    }
    private void OnClickEscape(InputAction.CallbackContext context)
    {
        if (Managers.UI.HasPopUpUI())
            Managers.UI.ClosePopupUI();
    }

}
