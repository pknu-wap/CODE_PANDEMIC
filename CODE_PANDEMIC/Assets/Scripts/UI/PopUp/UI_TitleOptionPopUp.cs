using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleOptionPopUp : UI_PopUp
{   
    enum GameObejcts
    {
        OptionPanel
    }
    enum Buttons
    {
        VideoButton,
        SoundButton,
        ExitButton
    }
    [SerializeField]
    GameObject _videoButton;

    [SerializeField]
    GameObject _soundButton;

    [SerializeField]
    GameObject _exitButton;    

    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindObject(typeof(GameObejcts));
        BindButton(typeof(Buttons));
        
        _videoButton = GetButton((int)Buttons.VideoButton).gameObject;
        _soundButton=GetButton((int)Buttons.SoundButton).gameObject;
        _exitButton = GetButton((int)(Buttons.ExitButton)).gameObject;

        BindEvent(_videoButton,OnClickVideoButton);
        BindEvent(_soundButton, OnClickSoundButton);
        BindEvent(_exitButton, OnClickExitButton);
        return true;
    }

    #region EventHandler
    void OnClickVideoButton()
    {
        Debug.Log("video");
    }
    void OnClickSoundButton()
    {
        Managers.UI.ShowPopupUI<UI_SoundPopUp>();
    }
    void OnClickExitButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
    #endregion
}
