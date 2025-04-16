using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_VideoPopUp : UI_PopUp
{
    enum Texts
    {
        ResolutionText
    }
    enum Buttons
    {
        PrevResolutionButton,
        NextResolutionButton,
        FullScreenButton,
        WindowScreenButton,
        ExitButton,
        SelectButton
    }

    GameObject _prevButton;
    GameObject _nextButton;
    GameObject _fullScreenButton;
    GameObject _windowScreenButton;
    GameObject _exitButton;
    GameObject _selectButton;

    TextMeshProUGUI _resolutionText;  

    private int _resolutionIndex = -1;
    private List<Resolution> _resolutions = new List<Resolution>();

    public override bool Init()
    {
        if (base.Init() == false) return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        _resolutionText = GetText((int)Texts.ResolutionText); 
        // �����ϴ� �ػ� ��� �߰�
        _resolutions.Add(new Resolution { width = 1280, height = 720 });
        _resolutions.Add(new Resolution { width = 1280, height = 800 });
        _resolutions.Add(new Resolution { width = 1440, height = 900 });
        _resolutions.Add(new Resolution { width = 1600, height = 900 });
        _resolutions.Add(new Resolution { width = 1680, height = 1050 });
        _resolutions.Add(new Resolution { width = 1920, height = 1080 });
  
        _prevButton = GetButton((int)Buttons.PrevResolutionButton).gameObject;
        _nextButton = GetButton((int)Buttons.NextResolutionButton).gameObject;
        _exitButton = GetButton((int)Buttons.ExitButton).gameObject;
        _fullScreenButton = GetButton((int)Buttons.FullScreenButton).gameObject;
        _windowScreenButton = GetButton((int)Buttons.WindowScreenButton).gameObject;
        _selectButton = GetButton((int)Buttons.SelectButton).gameObject;

        // ��ư �̺�Ʈ ���ε�
        BindEvent(_exitButton, OnClickExitButton);
        BindEvent(_fullScreenButton, () => OnClickScreenButton(true));
        BindEvent(_windowScreenButton, () => OnClickScreenButton(false));
        BindEvent(_prevButton, () => OnClickResolutionButton(-1));  // ���� �ػ� ��ư
        BindEvent(_nextButton, () => OnClickResolutionButton(1));   // ���� �ػ� ��ư
        BindEvent(_selectButton, OnClickSelectButton);
      
        _resolutionIndex = _resolutions.FindIndex(r => r.width == Screen.width && r.height == Screen.height);

        // �ػ󵵸� ã�� ������ ��� �⺻���� 1920x1080���� ����
        if (_resolutionIndex == -1)
            _resolutionIndex = _resolutions.FindIndex(r => r.width == 1920 && r.height == 1080);

        UpdateResolutionUI();
        _selectButton.SetActive(false);
        return true;
    }

   
    private void ChangeResolution(int index)
    {
        _selectButton?.SetActive(true);
        _resolutionIndex += index;

        // �ػ� �ε����� ������ ����� �ʵ��� ����
        if (_resolutionIndex < 0)
            _resolutionIndex = _resolutions.Count - 1;
        else if (_resolutionIndex >= _resolutions.Count)
            _resolutionIndex = 0;
        // UI ������Ʈ
        UpdateResolutionUI();
    }

    private void UpdateResolutionUI()
    {
        Resolution res = _resolutions[_resolutionIndex];
        _resolutionText.text = $"{res.width} x {res.height}";
    }

    private void UpdateScreenModeButton(bool isFullScreen)
    {
        Debug.Log($"[Screen Mode] isFullScreen: {isFullScreen}, Actual: {Screen.fullScreen}");
   
        GetButton((int)Buttons.FullScreenButton).image.color = isFullScreen ? new Color(0.588f, 0.588f, 0.588f) : Color.white;
        GetButton((int)Buttons.WindowScreenButton).image.color = isFullScreen ? Color.white : new Color(0.588f, 0.588f, 0.588f);
    }
    private void SelectSettings()
    {
       
        Resolution res = _resolutions[_resolutionIndex];
        Managers.Game.SetResolutionMode(res);

    }
    #region EventHandler

    private void OnClickExitButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    
    private void OnClickResolutionButton(int index)
    {
        ChangeResolution(index);
    }
  
    private void OnClickScreenButton(bool isFullScreen)
    {
        Managers.Game.SetScreenMode(isFullScreen);
        UpdateScreenModeButton(isFullScreen);
    }

    private void OnClickSelectButton()
    {
        SelectSettings();
    }

    #endregion
}
