using UnityEngine;
using TMPro;

public class PZ_Password_InputUI : UI_Base
{
    private RectTransform _rectTransform;
    private RectTransform _textRectTransform;

    private TextMeshProUGUI _textMeshPro;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        _textRectTransform = _textMeshPro.gameObject.GetComponent<RectTransform>();

        // 크기 세팅
        _rectTransform.anchorMin = new Vector2(0.5f, 1f);
        _rectTransform.anchorMax = new Vector2(0.5f, 1f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = new Vector2(0, -120);
        _rectTransform.sizeDelta = new Vector2(600, 150);

        // 비밀 번호 텍스트 세팅
        _textRectTransform.anchoredPosition = Vector2.zero;
        _textRectTransform.sizeDelta = new Vector2(500, 50);
        _textRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _textRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    // 화면에 출력할 현재 입력된 번호 출력
    public void SetPasswordText(string passwordText)
    {
        Debug.Log("Check Password : " + passwordText);

        _textMeshPro.text = passwordText;
    }
}