using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameStartPopUp : UI_PopUp
{
    enum Buttons
    {
        LoadGameButton,
        NewGameButton,
        ExitButton
    }
    enum Texts
    {
        Description
    }
    enum GameObjects
    {
        Content
    }
    
    GameObject _startButton;
    GameObject _exitButton;
    GameObject _newGameButton;
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));

        _startButton = GetButton((int)Buttons.LoadGameButton).gameObject;
        _exitButton = GetButton((int)Buttons.ExitButton).gameObject;
        _newGameButton = GetButton((int)Buttons.NewGameButton).gameObject;

        BindEvent(_startButton, OnClickStartButton);
        BindEvent(_exitButton, OnClickExitButton);
        BindEvent(_newGameButton, OnClickNewGameButton);

        SetUpPopup(Managers.Game.HasFile());

        return true;
    }

    private void GameStart()
    {
        Managers.Game.LoadGame();
        Managers.Game.SaveGame();
        Managers.UI.FadeOut(() =>
        {
            Managers.Scene.ChangeScene(Define.SceneType.GameScene);
        });
    }
    public void SetUpPopup(bool hasSaveData)
    {
        string desc = hasSaveData
            ? "진행중이던 파일이 존재합니다.\n 계속 진행하시겠습니까?"
            : "게임을 진행하시겠습니까?";

        GetText((int)Texts.Description).text = desc;

        _startButton.SetActive(hasSaveData);
        _newGameButton.SetActive(true);
    }

    #region EventHandler

    private void OnClickStartButton()
    {
        GameStart();
        Managers.UI.ClosePopupUI(this);
    }
    private void OnClickExitButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
    private void OnClickNewGameButton()
    {
        if (Managers.Game.HasFile())
            Managers.Game.DeleteSaveData();
        
        GameStart();
        Managers.UI.ClosePopupUI(this);

    }
    #endregion

}
