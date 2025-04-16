using UnityEngine;
using System.Collections.Generic;

public class PZ_LightOut_Board : PZ_Puzzle_Main
{
    #region Base

    private List<PZ_LightOut_Button> _lightOutButtonList = new List<PZ_LightOut_Button>(); // 소환된 버튼들 관리

    private int _buttonMaxCount = 25; // Light 버튼 개수

    private void Start()
    {
        Managers.UI.SetCanvas(gameObject);

        GetSpawnedButtons();
    }

    #endregion

    #region Setting

    // Light Button 가져오기
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

    // 리셋
    public void ResetButtons()
    {
        for (int index = 0; index < _buttonMaxCount; index++)
        {
            _lightOutButtonList[index].ShuffleButtonState();
        }
    }

    // 클릭한 버튼과 상하좌우의 버튼들의 상태를 변경시키는 함수
    public void ChangeButtonsState(int currentIndex)
    {
        // 현재 버튼 위치 상태 변경
        _lightOutButtonList[currentIndex].ChangeButtonState();

        // 상 방향 유효 체크
        if (currentIndex - 5 >= 0)
        {
            _lightOutButtonList[currentIndex - 5].ChangeButtonState();
        }

        // 하 방향 유효 체크
        if (currentIndex + 5 <= _buttonMaxCount - 1)
        {
            _lightOutButtonList[currentIndex + 5].ChangeButtonState();
        }

        // 좌 방향 유효 체크
        if (currentIndex != 0 && currentIndex != 5 && currentIndex != 10 && currentIndex != 15 && currentIndex != 20)
        {
            _lightOutButtonList[currentIndex - 1].ChangeButtonState();
        }

        // 우 방향 유효 체크
        if (currentIndex != 4 && currentIndex != 9 && currentIndex != 14 && currentIndex != 19 && currentIndex != 24)
        {
            _lightOutButtonList[currentIndex + 1].ChangeButtonState();
        }
    }

    #endregion

    #region Clear

    // 버튼들이 전부 올바른 상태인지 체크
    public void CheckButtonsCorrect()
    {
        if (_lightOutButtonList.FindAll(button => button.IsButtonCorrect() == true).Count == _buttonMaxCount)
        {
            PuzzleClear();
        }
    }

    // 퍼즐 클리어
    protected override void PuzzleClear()
    {
        Debug.LogWarning("Light Out Puzzle Clear!!!");

        _puzzleOwner.ClearPuzzle();
    }

    #endregion
}