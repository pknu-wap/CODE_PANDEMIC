using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PZ_Password_Board : UI_PopUp
{
    private PZ_Password_InputUI _passwordInputUI;

    private RectTransform _rectTransform; // ����
    private Image _image; // ����
    private GridLayoutGroup _layoutGroup; // ����

    private List<PZ_Password_Button> _buttonList = new List<PZ_Password_Button>(); // ������ ��ư ����Ʈ

    private string _correctPassword = "IUYC"; // ���� ��� ��ȣ
    private string _inputPassword; // �Է� �޴� ��� ��ȣ

    private void Init()
    {
        Managers.Resource.Instantiate("PZ_Password_InputUI_Prefab", GetComponentInParent<Canvas>().transform, (spawnedInputUI) =>
        {
            _passwordInputUI = spawnedInputUI.GetComponent<PZ_Password_InputUI>();
        });

        _rectTransform = GetComponent<RectTransform>();
        _layoutGroup = GetComponent<GridLayoutGroup>();

        _image = GetComponent<Image>();

        Managers.Resource.LoadAsync<Sprite>("PZ_Password_Board_Sprite", (getSprite) =>
        {
            _image.sprite = getSprite;
        });

        // ��� ��ȣ �� �⺻ ����
        _rectTransform.anchorMin = new Vector2(0.5f, 0f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = new Vector2(0, 420);
        _rectTransform.sizeDelta = new Vector2(600, 800);

        // ������ ��ư���� ��ġ�� ����
        _layoutGroup.padding.left = 100;
        _layoutGroup.padding.right = 100;
        _layoutGroup.padding.top = 100;
        _layoutGroup.padding.bottom = 100;
        _layoutGroup.cellSize = new Vector2(120, 120);
        _layoutGroup.spacing = new Vector2(20, 20);
    }

    private void Start()
    {
        Init();

        GetSpawnedButtons();
    }

    // ��ư ��������
    private void GetSpawnedButtons()
    {
        for (int index = 0; index < 12; index++)
        {
            Transform childButton = transform.GetChild(index);
            PZ_Password_Button spawnedButton = childButton.gameObject.GetComponent<PZ_Password_Button>();
            spawnedButton.ButtonSetup();
            _buttonList.Add(spawnedButton);
        }
    }

    // ��� ��ȣ �Է�
    public void InputPassword(string selectedWord)
    {
        _inputPassword += selectedWord;

        _passwordInputUI.SetPasswordText(_inputPassword);

        CheckPuzzleClear();
    }

    // ��� ��ȣ ��ġ üũ
    private void CheckPuzzleClear()
    {
        if (_inputPassword == _correctPassword)
        {
            Debug.LogWarning("Password Puzzle Clear!!!");
            // ���⿡ ���� Ŭ���� ���� ����
        }

        // ��� ��ȣ �ʱ�ȭ
        if (_inputPassword.Length >= 4)
        {
            _inputPassword = "";
        }
    }
}