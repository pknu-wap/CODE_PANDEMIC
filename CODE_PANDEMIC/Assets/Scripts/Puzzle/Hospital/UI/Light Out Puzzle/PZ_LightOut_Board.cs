using System.Collections.Generic;

public class PZ_LightOut_Board : PZ_Puzzle_UI_Main
{
    #region Base

    private List<PZ_LightOut_Button> _lightOutButtonList = new List<PZ_LightOut_Button>();

    private int _buttonMaxCount = 9;

    private void Start()
    {
        GetSpawnedButtons();
    }

    #endregion

    #region Setting

    private void GetSpawnedButtons()
    {
        GetComponentsInChildren(false, _lightOutButtonList);

        for (int index = 0; index < _buttonMaxCount; index++)
        {
            _lightOutButtonList[index].Setting(index);
        }
    }

    #endregion

    #region Click

    public void ResetButtons()
    {
        for (int index = 0; index < _buttonMaxCount; index++)
        {
            _lightOutButtonList[index].ShuffleButtonState();
        }
    }

    public void ChangeButtonsState(int currentIndex)
    {
        _lightOutButtonList[currentIndex].ChangeButtonState();

        if (currentIndex - 3 >= 0)
        {
            _lightOutButtonList[currentIndex - 3].ChangeButtonState();
        }

        if (currentIndex + 3 <= _buttonMaxCount - 1)
        {
            _lightOutButtonList[currentIndex + 3].ChangeButtonState();
        }

        if (currentIndex != 0 && currentIndex != 3 && currentIndex != 6)
        {
            _lightOutButtonList[currentIndex - 1].ChangeButtonState();
        }

        if (currentIndex != 2 && currentIndex != 5 && currentIndex != 8)
        {
            _lightOutButtonList[currentIndex + 1].ChangeButtonState();
        }
    }

    #endregion

    #region Clear

    public void CheckButtonsCorrect()
    {
        if (_lightOutButtonList.FindAll(button => button.IsButtonCorrect() == true).Count == _buttonMaxCount)
        {
            PuzzleClear();
        }
    }

    #endregion
}