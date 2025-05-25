using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_TitleScene : UI_Scene
{   
    enum Texts
    {
        StartText,
        OptionText,
        ExitText
    }
    enum Buttons
    {
        StartButton,
        OptionButton,
        ExitButton,
        HelpButton
    }
  
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.StartButton).gameObject, OnClickStartButton);
        BindEvent(GetButton((int)Buttons.OptionButton).gameObject, OnClickOptionButton);
        BindEvent(GetButton((int)Buttons.ExitButton).gameObject, OnClickExitButton);
        BindEvent(GetButton((int)Buttons.HelpButton).gameObject, OnClickHelpButton);

        return true;
    }



    #region EventHandler
    private void OnClickExitButton()
    {
       Managers.Game.QuitGame();
    }
    private void OnClickOptionButton()
    {
        Managers.UI.ShowPopupUI<UI_TitleOptionPopUp>();
        
    }
    private void OnClickStartButton()
    {
        Managers.UI.ShowPopupUI<UI_GameStartPopUp>();
    }

    private void OnClickHelpButton()
    {
        Managers.UI.ShowPopupUI<UI_TutorialPopUp>("UI_KeyboardTutorialPopUp");
    }

    #endregion
}
