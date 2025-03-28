using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PZ_Password_Board : UI_PopUp
{
    private GameObject _inputUIPrefab;
    private PZ_Password_InputUI _passwordInputUI;

    private RectTransform _rectTransform; // ����
    private GridLayoutGroup _layoutGroup; // ����

    private List<PZ_Password_Button> _buttonList = new List<PZ_Password_Button>(); // ������ ��ư ����Ʈ

    private string _correctPassword = "1234"; // ���� ��� ��ȣ
    private string _inputPassword; // �Է� �޴� ��� ��ȣ

    private bool Init()
    {
        Managers.Resource.LoadAsync<GameObject>("PZ_Password_InputUI", (inputUIPrefab) =>
        {
            _inputUIPrefab = inputUIPrefab;
        });

        if (!_inputUIPrefab)
        {
            return false;
        }

        GameObject spawnedInputUI = Instantiate(_inputUIPrefab, transform);
        _passwordInputUI = spawnedInputUI.GetComponent<PZ_Password_InputUI>();

        _rectTransform = GetComponent<RectTransform>();
        _layoutGroup = GetComponent<GridLayoutGroup>();

        if (!_rectTransform || !_layoutGroup || !_passwordInputUI)
        {
            return false;
        }

        // ��� ��ȣ �� �⺻ ����
        _rectTransform.anchorMin = new Vector2(0.5f, 0f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = new Vector2(0, 420);
        _rectTransform.sizeDelta = new Vector2(600, 800);

        // ������ ��ư���� ��ġ�� ����
        _layoutGroup.padding.left = 10;
        _layoutGroup.padding.right = 10;
        _layoutGroup.padding.top = 10;
        _layoutGroup.padding.bottom = 10;
        _layoutGroup.cellSize = new Vector2(180, 180);
        _layoutGroup.spacing = new Vector2(10, 10);

        return true;
    }

    private void Start()
    {
        if (!Init())
        {
            return;
        }

        SpawnButtons();
    }

    // ��ư ����
    private void SpawnButtons()
    {
        for (int i = 1; i <= 12; i++)
        {
            // Grid Layout Group�� Ȱ���� ���忡 Ÿ���� �����ϸ� �ڵ����� ��ġ�ǰ� ��
            Managers.Resource.Instantiate("PZ_Password_Button", transform, (spawnedButtonObject) =>
            {
                PZ_Password_Button spawnedButton = spawnedButtonObject.GetComponent<PZ_Password_Button>();
                _buttonList.Add(spawnedButton);
                spawnedButton.ButtonSetup(i);
            });           
        }
    }

    // ��� ��ȣ �Է�
    public void InputPassword(int selectedNumber)
    {
        _inputPassword += selectedNumber.ToString();

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