using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class PZ_Password_InputUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform _rectTransform;

    [SerializeField]
    private TextMeshProUGUI _passwordText;

    [SerializeField]
    private RectTransform _textRectTransform;

    private void Start()
    {
        if(!_passwordText)
        {
            Debug.Log("스타트에서도 없음");
        }
        else
        {
            Debug.Log("스타트에서는 있음");
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

        //GameObject spawnedText = Instantiate(_passwordTextPrefab, transform);
        //_passwordText = spawnedText.GetComponent<TextMeshProUGUI>();
    }

    // 화면에 출력할 현재 입력된 번호 출력
    public void SetPasswordText(string passwordText)
    {
        Debug.Log("Check Password : " + passwordText);

        if (_passwordText)
        {
            _passwordText.text = passwordText;
        }
        else
        {
            Debug.Log("할당 안됨");
        }
    }
}