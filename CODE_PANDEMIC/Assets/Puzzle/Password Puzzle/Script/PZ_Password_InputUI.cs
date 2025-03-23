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
            Debug.Log("��ŸƮ������ ����");
        }
        else
        {
            Debug.Log("��ŸƮ������ ����");
        }

        // ũ�� ����
        _rectTransform.anchorMin = new Vector2(0.5f, 1f);
        _rectTransform.anchorMax = new Vector2(0.5f, 1f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = new Vector2(0, -120);
        _rectTransform.sizeDelta = new Vector2(600, 150);

        // ��� ��ȣ �ؽ�Ʈ ����
        _textRectTransform.anchoredPosition = Vector2.zero;
        _textRectTransform.sizeDelta = new Vector2(500, 50);
        _textRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _textRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);

        //GameObject spawnedText = Instantiate(_passwordTextPrefab, transform);
        //_passwordText = spawnedText.GetComponent<TextMeshProUGUI>();
    }

    // ȭ�鿡 ����� ���� �Էµ� ��ȣ ���
    public void SetPasswordText(string passwordText)
    {
        Debug.Log("Check Password : " + passwordText);

        if (_passwordText)
        {
            _passwordText.text = passwordText;
        }
        else
        {
            Debug.Log("�Ҵ� �ȵ�");
        }
    }
}