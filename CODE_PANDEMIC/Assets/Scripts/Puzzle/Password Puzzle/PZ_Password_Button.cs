using UnityEngine.UI;
using TMPro;

public class PZ_Password_Button : UI_Base
{
    private TextMeshProUGUI _buttonNumberText; // �׽�Ʈ�� ���� ��ư ��ȣ ����� ���� text
    private int _buttonNumber; // ���� ��ư ��ȣ

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

    // ��ư�� ���� �� ����
    public void ButtonSetup(int index)
    {
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

    // ��ư Ŭ�� �̺�Ʈ
    public void OnButtonClick()
    {
        // �ش� ��ư�� ���� �Է�
        _passwordBoard.InputPassword(_buttonNumber);
    }
}