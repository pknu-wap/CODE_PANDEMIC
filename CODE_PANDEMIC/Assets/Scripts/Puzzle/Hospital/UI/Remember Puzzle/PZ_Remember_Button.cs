using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PZ_Remember_Button : UI_Base
{
    private Image _image;
    private Sprite _normalSprite;
    private Sprite _pressedSprite;

    private PZ_Remember_Board _rememberBoard;

    private int _buttonNumber;

    public bool _isShowingEvent = false;

    public void SettingButton(PZ_Remember_Board rememberBoard, int buttonNumber)
    {
        _image = GetComponent<Image>();

        _rememberBoard = rememberBoard;

        _buttonNumber = buttonNumber;

        Managers.Resource.LoadAsync<Sprite>("PZ_Remember_Green_Sprite", (getSprite) =>
        {
            _normalSprite = getSprite;

            _image.sprite = _normalSprite;
        });

        Managers.Resource.LoadAsync<Sprite>("PZ_Remember_Red_Sprite", (getSprite) =>
        {
            _pressedSprite = getSprite;
        });

        BindEvent(gameObject, OnButtonClick, Define.UIEvent.Click);
    }

    public IEnumerator ChangeButtonColor()
    {
        _image.sprite = _pressedSprite;

        yield return CoroutineHelper.WaitForSecondsRealtime(0.4f);

        _image.sprite = _normalSprite;
    }

    public void OnButtonClick()
    {
        if (_isShowingEvent)
        {
            return;
        }

        StartCoroutine(ChangeButtonColor());

        _rememberBoard.CheckPuzzleClear(_buttonNumber);
    }
}