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

        _textRectTransform.anchoredPosition = Vector2.zero;
        _textRectTransform.sizeDelta = new Vector2(500, 50);
        _textRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _textRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }

    public void SetPasswordText(string passwordText)
    {
        _textMeshPro.text = passwordText;
    }
}