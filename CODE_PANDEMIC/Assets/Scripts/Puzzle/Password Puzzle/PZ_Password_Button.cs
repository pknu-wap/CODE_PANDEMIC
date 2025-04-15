using TMPro;

public class PZ_Password_Button : UI_Base
{
    private TextMeshProUGUI _buttonNumberText; // 현재 버튼 text
    private int _buttonNumber; // 현재 버튼 번호

    private PZ_Password_Board _passwordBoard;

    public int ButtonNumber
    {
        get { return _buttonNumber; }

        set { _buttonNumber = value; }
    }

    // 버튼의 고유 값 설정
    public void ButtonSetup()
    {
        _passwordBoard = GetComponentInParent<PZ_Password_Board>();

        _buttonNumberText = GetComponentInChildren<TextMeshProUGUI>();

        BindEvent(gameObject, OnButtonClick);
    }

    // 버튼 클릭 이벤트
    public void OnButtonClick()
    {
        // 해당 버튼의 값을 입력
        _passwordBoard.InputPassword(_buttonNumberText.text);
    }
}