using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PZ_Password_InputUI : UI_Base
{
    private RectTransform _rectTransform;
    private RectTransform _textRectTransform;

    private Image _image;

    private TextMeshProUGUI _textMeshPro;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        _image = GetComponent<Image>();

        _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        _textRectTransform = _textMeshPro.gameObject.GetComponent<RectTransform>();

        Managers.Resource.LoadAsync<Sprite>("PZ_Password_InputUI_Sprite", (getSprite) =>
        {
            _image.sprite = getSprite;
        });

        // ũ�� ����
        _rectTransform.anchorMin = new Vector2(0.5f, 1f);
        _rectTransform.anchorMax = new Vector2(0.5f, 1f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = new Vector2(0, -140);
        _rectTransform.sizeDelta = new Vector2(700, 250);

        // ��� ��ȣ �ؽ�Ʈ ����
        _textRectTransform.anchoredPosition = Vector2.zero;
        _textRectTransform.sizeDelta = new Vector2(500, 50);
        _textRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _textRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    // ȭ�鿡 ����� ���� �Էµ� ��ȣ ���
    public void SetPasswordText(string passwordText)
    {
        Debug.Log("Check Password : " + passwordText);

        _textMeshPro.text = passwordText;
    }
}