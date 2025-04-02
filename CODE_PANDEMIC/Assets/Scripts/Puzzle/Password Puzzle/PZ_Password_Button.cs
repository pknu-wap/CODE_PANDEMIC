using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PZ_Password_Button : UI_Base
{
    private Image _image;

    private TextMeshProUGUI _buttonNumberText; // 테스트용 현재 버튼 번호 출력을 위한 text
    private int _buttonNumber; // 현재 버튼 번호

    private PZ_Password_Board _passwordBoard;

    public int ButtonNumber
    {
        get { return _buttonNumber; }

        set
        {
            _buttonNumber = value;
            _buttonNumberText.text = _buttonNumber.ToString();
        }
    }

    // 버튼의 고유 값 설정
    public void ButtonSetup(int index)
    {
        _image = GetComponent<Image>();

        Managers.Resource.LoadAsync<Sprite>("PZ_Password_Button_Sprite", (getSprite) =>
        {
            _image.sprite = getSprite;
        });

        _passwordBoard = GetComponentInParent<PZ_Password_Board>();

        _buttonNumberText = GetComponentInChildren<TextMeshProUGUI>();

        if (index == 10 || index == 12)
        {
            GetComponent<Image>().enabled = false;
            _buttonNumberText.enabled = false;
        }
        else if (index == 11)
        {
            ButtonNumber = 0;
        }
        else
        {
            ButtonNumber = index;
        }

        BindEvent(gameObject, OnButtonClick);
    }

    // 버튼 클릭 이벤트
    public void OnButtonClick()
    {
        // 해당 버튼의 값을 입력
        _passwordBoard.InputPassword(_buttonNumber);
    }
}