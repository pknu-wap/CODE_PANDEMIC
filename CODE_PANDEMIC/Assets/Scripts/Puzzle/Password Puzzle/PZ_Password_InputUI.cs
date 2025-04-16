using UnityEngine;
using TMPro;

public class PZ_Password_InputUI : UI_PopUp
{
    private RectTransform _textRectTransform;

    private TextMeshProUGUI _textMeshPro;

    private void Start()
    {
        _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        _textRectTransform = _textMeshPro.gameObject.GetComponent<RectTransform>();

        // 비밀 번호 텍스트 세팅
        _textRectTransform.anchoredPosition = Vector2.zero;
        _textRectTransform.sizeDelta = new Vector2(500, 50);
        _textRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _textRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }

    // 화면에 출력할 현재 입력된 번호 출력
    public void SetPasswordText(string passwordText)
    {
        Debug.Log("Check Password : " + passwordText);

        _textMeshPro.text = passwordText;
    }
}