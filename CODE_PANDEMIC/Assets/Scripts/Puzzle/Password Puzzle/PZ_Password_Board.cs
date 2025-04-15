using UnityEngine;
using System.Collections.Generic;

public class PZ_Password_Board : PZ_Puzzle_Main
{
    #region Base

    private PZ_Password_InputUI _passwordInputUI;

    private List<PZ_Password_Button> _buttonList = new List<PZ_Password_Button>(); // 생성한 버튼 리스트

    private string _correctPassword = "IUYC"; // 정답 비밀 번호
    private string _inputPassword; // 입력 받는 비밀 번호

    private void Start()
    {
        Managers.UI.SetCanvas(gameObject);

        _passwordInputUI = GetComponentInChildren<PZ_Password_InputUI>();

        GetSpawnedButtons();
    }

    #endregion

    #region Setting

    // 버튼 가져오기
    private void GetSpawnedButtons()
    {
        GetComponentsInChildren(false, _buttonList);

        for (int index = 0; index < 12; index++)
        {
            _buttonList[index].ButtonSetup();
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

        _puzzleOwner.ClearPuzzle();
    }

    #endregion
}