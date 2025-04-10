using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PZ_Password_Button : UI_Base
{
    private Image _image;

    private TextMeshProUGUI _buttonNumberText; // ���� ��ư text
    private int _buttonNumber; // ���� ��ư ��ȣ

    private PZ_Password_Board _passwordBoard;

    public int ButtonNumber
    {
        get { return _buttonNumber; }

        set { _buttonNumber = value; }
    }

    // ��ư�� ���� �� ����
    public void ButtonSetup()
    {
        _image = GetComponent<Image>();

        Managers.Resource.LoadAsync<Sprite>("PZ_Password_Button_Sprite", (getSprite) =>
        {
            _image.sprite = getSprite;
        });

        _passwordBoard = GetComponentInParent<PZ_Password_Board>();

        _buttonNumberText = GetComponentInChildren<TextMeshProUGUI>();

        BindEvent(gameObject, OnButtonClick);
    }

    // ��ư Ŭ�� �̺�Ʈ
    public void OnButtonClick()
    {
        // �ش� ��ư�� ���� �Է�
        _passwordBoard.InputPassword(_buttonNumberText.text);
    }
}