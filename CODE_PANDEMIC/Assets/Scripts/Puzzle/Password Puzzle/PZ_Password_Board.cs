using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PZ_Password_Board : PZ_Puzzle_Base
{
    #region Base

    private PZ_Password_InputUI _passwordInputUI;

    private GridLayoutGroup _layoutGroup; // 세팅

    private List<PZ_Password_Button> _buttonList = new List<PZ_Password_Button>(); // 생성한 버튼 리스트

    private string _correctPassword = "IUYC"; // 정답 비밀 번호
    private string _inputPassword; // 입력 받는 비밀 번호

    private void Init()
    {
        SetComponents();

        Managers.Resource.Instantiate("PZ_Password_InputUI_Prefab", GetComponentInParent<Canvas>().transform, (spawnedInputUI) =>
        {
            _passwordInputUI = spawnedInputUI.GetComponent<PZ_Password_InputUI>();
        });

        _layoutGroup = GetComponent<GridLayoutGroup>();

        Managers.Resource.LoadAsync<Sprite>("PZ_Password_Board_Sprite", (getSprite) =>
        {
            _image.sprite = getSprite;
        });

        // 비밀 번호 판 기본 세팅
        _rectTransform.anchorMin = new Vector2(0.5f, 0f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = new Vector2(0, 420);
        _rectTransform.sizeDelta = new Vector2(600, 800);

        // 생성할 버튼들을 배치할 세팅
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

    private void OnDestroy()
    {
        if (_passwordInputUI)
        {
            Destroy(_passwordInputUI.gameObject);
        }
    }

    #endregion

    #region Setting

    // 버튼 가져오기
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

    #endregion

    #region Password

    // 비밀 번호 입력
    public void InputPassword(string selectedWord)
    {
        _inputPassword += selectedWord;

        _passwordInputUI.SetPasswordText(_inputPassword);

        CheckPuzzleClear();
    }

    #endregion

    #region Clear

    // 비밀 번호 일치 체크
    private void CheckPuzzleClear()
    {
        if (_inputPassword == _correctPassword)
        {
            PuzzleClear();

            return;
        }

        // 비밀 번호 초기화
        if (_inputPassword.Length >= 4)
        {
            _inputPassword = "";
        }
    }

    // 퍼즐 클리어
    protected override void PuzzleClear()
    {
        Debug.LogWarning("Password Puzzle Clear!!!");

        Destroy(_passwordInputUI.gameObject);
        _puzzleOwner.ClearPuzzle();
    }

    #endregion
}