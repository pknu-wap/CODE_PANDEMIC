using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory.UI.UI_Inventory;

public class UI_RewardPopUp : UI_PopUp
{
    enum GameObjects
    {
        Content,
    }
    enum Buttons
    {
        ExitButton
    }
    GameObject _exitButton;
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        
        _exitButton = GetButton((int)(Buttons.ExitButton)).gameObject;

       
        BindEvent(_exitButton, OnClickExitButton);
        return true;
    }

    private void OnClickExitButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
