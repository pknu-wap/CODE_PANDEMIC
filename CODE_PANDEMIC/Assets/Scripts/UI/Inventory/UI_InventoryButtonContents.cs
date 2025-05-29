using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryButtonContents : UI_Base
{
    enum Buttons
    {
        InventoryTutorialButton,
        WeaponTutorialButton
    }
    public override bool Init()
    {
        if (base.Init() == false) return false;

        BindButton(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.InventoryTutorialButton).gameObject, OnClickInventoryTutorialButton);
        BindEvent(GetButton((int)Buttons.WeaponTutorialButton).gameObject, OnClickWeaponTutorialButton);

        return true;
    }

    private void OnClickWeaponTutorialButton()
    {
        Debug.Log("TODO");
    }

    private void OnClickInventoryTutorialButton()
    {
        Managers.UI.ShowPopupUI<UI_TutorialPopUp>("UI_InventoryTutorialPopUp");
    }
}
