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
        TitleText,
        StartText,
        OptionText,
        ExitText
    }
    enum Buttons
    {
        StartButton,
        OptionButton,
        ExitButton
    }
    GameObject _startButton;
    GameObject _optionButton;
    GameObject _exitButton;

    
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

       _startButton= GetButton((int)Buttons.StartButton).gameObject;
       _optionButton=GetButton((int)Buttons.OptionButton).gameObject;
       _exitButton=GetButton((int)Buttons.ExitButton).gameObject;

        BindEvent(_startButton, OnClickStartButton);
        BindEvent(_optionButton, OnClickOptionButton);
        BindEvent(_exitButton, OnClickExitButton);

        return true;
    }

    #region EventHandler
    private void OnClickExitButton()
    {
       
    }
    private void OnClickOptionButton()
    {
        Managers.UI.ShowPopupUI<UI_TitleOptionPopUp>();
        
    }
    private void OnClickStartButton()
    {
        Managers.Scene.ChangScene(Define.SceneType.GameScene);
    }
    #endregion
}
