using TMPro;

public class PZ_Password_Button : UI_Base
{
    private TextMeshProUGUI _buttonNumberText;
    private int _buttonNumber;

    private PZ_Password_Board _passwordBoard;

    public int ButtonNumber
    {
        get { return _buttonNumber; }

        set { _buttonNumber = value; }
    }

    public void ButtonSetup()
    {
        _passwordBoard = GetComponentInParent<PZ_Password_Board>();

        _buttonNumberText = GetComponentInChildren<TextMeshProUGUI>();

        BindEvent(gameObject, OnButtonClick, Define.UIEvent.Click);
    }

    public void OnButtonClick()
    {
        _passwordBoard.InputPassword(_buttonNumberText.text);
    }
}