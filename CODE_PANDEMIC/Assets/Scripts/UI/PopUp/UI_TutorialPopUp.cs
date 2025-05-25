using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TutorialPopUp :UI_PopUp
{
    [SerializeField] GameObject _nextPopUp;

    enum Buttons
    {
        ExitButton
    }
    GameObject _exitButton;
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindButton(typeof(Buttons));
        _exitButton = GetButton((int)Buttons.ExitButton).gameObject;
        BindEvent(_exitButton,OnClickExitButton);
        return true;
    }
    private void OnEnable()
    {
        Managers.Game.PauseGame();
    }
    private void OnDisable()
    {
        Managers.Game.ResumeGame();
        if(_nextPopUp!=null)
        {
            Managers.UI.ShowPopupUI<UI_TutorialPopUp>(_nextPopUp.name);
        }
    }
    private void OnClickExitButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
