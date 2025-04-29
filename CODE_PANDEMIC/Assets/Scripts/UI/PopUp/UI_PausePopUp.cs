using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PausePopUp :UI_PopUp
{
    enum Buttons
    {
        ExitButton,
        VideoButton,
        SoundButton,
        TitleButton,
    }
    [SerializeField]
    GameObject _exitButton;
    [SerializeField]
    GameObject _videoButton;
    [SerializeField]
    GameObject _soundButton;
    [SerializeField]
    GameObject _titleButton;

    private void OnEnable()
    {
       Debug.Log("Pause");
        Managers.Game.PauseGame();
    }
    private void OnDisable()
    {
       Debug.Log("Resume");
       Managers.Game.ResumeGame();
    }
    public override bool Init()
    {
        if (base.Init()==false) { return false; }

        BindButton(typeof(Buttons));
        _exitButton = GetButton((int)Buttons.ExitButton).gameObject;
        _videoButton = GetButton((int)Buttons.VideoButton).gameObject;
        _soundButton = GetButton((int)Buttons.SoundButton).gameObject;
        _titleButton = GetButton((int)Buttons.TitleButton).gameObject;

        BindEvent(_exitButton, OnClickExitButton);
        BindEvent(_videoButton, OnClickVideoButton);
        BindEvent(_soundButton, OnClickSoundButton);
        BindEvent(_titleButton, OnClickTitleButton);
        return true;
    }
   

    #region EventHandler

    private void OnClickExitButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
    private void OnClickVideoButton()
    {
        Managers.UI.ShowPopupUI<UI_VideoPopUp>();
    }
    private void OnClickSoundButton()
    {
        Managers.UI.ShowPopupUI<UI_SoundPopUp>();
    }
    private void OnClickTitleButton()
    {
        Managers.UI.ClosePopupUI(this);
        Managers.Game.SaveGame();
        Managers.UI.FadeOut(() => { Managers.Scene.ChangeScene(Define.SceneType.TitleScene); });
     
    }
    
    #endregion
}
