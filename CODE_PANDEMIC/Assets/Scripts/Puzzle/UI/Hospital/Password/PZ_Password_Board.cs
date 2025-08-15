using System.Collections.Generic;

public class PZ_Password_Board : PZ_Puzzle_UI_Main
{
    #region Base

    private PZ_Password_InputUI _passwordInputUI;

    private List<PZ_Password_Button> _buttonList = new List<PZ_Password_Button>();

    private string _correctPassword = "IUYC";
    private string _inputPassword;

    private void Start()
    {
        _passwordInputUI = GetComponentInChildren<PZ_Password_InputUI>();

        GetSpawnedButtons();
    }

    #endregion

    #region Setting

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

    public void InputPassword(string selectedWord)
    {
        _inputPassword += selectedWord;

        _passwordInputUI.SetPasswordText(_inputPassword);

        CheckPuzzleClear();
    }

    #endregion

    #region Clear

    public override void CheckPuzzleClear()
    {
        if (_inputPassword == _correctPassword)
        {
            PuzzleClear();

            return;
        }

        if (_inputPassword.Length >= 4)
        {
            _inputPassword = "";
        }
    }

    #endregion
}