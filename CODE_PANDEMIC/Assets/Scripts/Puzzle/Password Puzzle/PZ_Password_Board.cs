using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PZ_Password_Board : UI_PopUp
{
    private GameObject _inputUIPrefab;
    private PZ_Password_InputUI _passwordInputUI;

    private RectTransform _rectTransform; // 세팅
    private GridLayoutGroup _layoutGroup; // 세팅

    private List<PZ_Password_Button> _buttonList = new List<PZ_Password_Button>(); // 생성한 버튼 리스트

    private string _correctPassword = "1234"; // 정답 비밀 번호
    private string _inputPassword; // 입력 받는 비밀 번호

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

        // 비밀 번호 판 기본 세팅
        _rectTransform.anchorMin = new Vector2(0.5f, 0f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = new Vector2(0, 420);
        _rectTransform.sizeDelta = new Vector2(600, 800);

        // 생성할 버튼들을 배치할 세팅
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

    // 버튼 스폰
    private void SpawnButtons()
    {
        for (int i = 1; i <= 12; i++)
        {
            // Grid Layout Group을 활용해 보드에 타일을 스폰하면 자동으로 배치되게 함
            Managers.Resource.Instantiate("PZ_Password_Button", transform, (spawnedButtonObject) =>
            {
                PZ_Password_Button spawnedButton = spawnedButtonObject.GetComponent<PZ_Password_Button>();
                _buttonList.Add(spawnedButton);
                spawnedButton.ButtonSetup(i);
            });           
        }
    }

    // 비밀 번호 입력
    public void InputPassword(int selectedNumber)
    {
        _inputPassword += selectedNumber.ToString();

        _passwordInputUI.SetPasswordText(_inputPassword);

        CheckPuzzleClear();
    }

    // 비밀 번호 일치 체크
    private void CheckPuzzleClear()
    {
        if (_inputPassword == _correctPassword)
        {
            Debug.LogWarning("Password Puzzle Clear!!!");
            // 여기에 퍼즐 클리어 로직 구현
        }

        // 비밀 번호 초기화
        if (_inputPassword.Length >= 4)
        {
            _inputPassword = "";
        }
    }
}