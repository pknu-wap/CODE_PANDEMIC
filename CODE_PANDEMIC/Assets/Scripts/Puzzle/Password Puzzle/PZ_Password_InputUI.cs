using UnityEngine;
using TMPro;

public class PZ_Password_InputUI : MonoBehaviour
{
    private RectTransform _rectTransform;
    private GameObject _passwordTextObject;
    private RectTransform _textRectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        Managers.Resource.Instantiate("PZ_Password_Text", transform, (_passwordText) =>
        {
            _passwordTextObject = _passwordText;
            _textRectTransform = _passwordText.GetComponent<RectTransform>();
        });

        if (!_rectTransform || _textRectTransform)
        {
            return;
        }

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

        _passwordTextObject.GetComponent<TextMeshProUGUI>().text = passwordText;
    }
}