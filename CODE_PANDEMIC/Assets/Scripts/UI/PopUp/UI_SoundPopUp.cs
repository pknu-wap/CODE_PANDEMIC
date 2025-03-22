using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SoundPopUp :UI_PopUp
{
    enum GameObjects
    {
        Texts,
        Sliders,
    }
    enum Sliders
    {
        MasterSlider,
        BgmSlider,
        EffectSlider
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
        BindSlider(typeof(Sliders));
        BindButton(typeof(Buttons));

        _exitButton = GetButton((int)Buttons.ExitButton).gameObject;
        BindEvent(_exitButton, OnClickExitButton);
        return true;
    }
    //TODO: SOUND MANAGER와 SLIDER의 연동 
    #region EventHandler 
    void OnClickExitButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
    #endregion
}
