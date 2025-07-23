using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PZ_Remember_Board : PZ_Puzzle_UI_Side
{
    private int _rememberCount = 5;
    private const int _buttonCount = 5;

    private List<PZ_Remember_Button> _buttonList = new List<PZ_Remember_Button>();

    private List<int> _correctNumbers = new List<int>();
    private int _currentIndex = 0;

    private PZ_Generator _owner;

    public void Setting(PZ_Generator owner, int selectedRememberCount)
    {
        _rememberCount = selectedRememberCount;

        _owner = owner;

        GetComponentsInChildren(false, _buttonList);

        for (int index = 0; index < _buttonCount; index++)
        {
            _buttonList[index].SettingButton(this, index);
        }

        StartCoroutine(StartRandomEvent());
    }

    private IEnumerator StartRandomEvent()
    {
        for (int index = 0; index < _buttonCount; index++)
        {
            _buttonList[index]._isShowingEvent = true;
        }

        yield return CoroutineHelper.WaitForSecondsRealtime(1.0f);

        for (int index = 0; index < _rememberCount; index++)
        {
            int randomInt = Random.Range(0, _buttonCount);

            StartCoroutine(_buttonList[randomInt].ChangeButtonColor());

            _correctNumbers.Add(randomInt);

            yield return CoroutineHelper.WaitForSecondsRealtime(0.6f);
        }

        for (int index = 0; index < _buttonCount; index++)
        {
            _buttonList[index]._isShowingEvent = false;
        }
    }

    public override void CheckPuzzleClear(int buttonNumber)
    {
        if (_correctNumbers[_currentIndex] != buttonNumber)
        {
            _currentIndex = 0;
            _correctNumbers.Clear();

            StartCoroutine(StartRandomEvent());

            return;
        }

        _currentIndex++;

        if (_currentIndex == _rememberCount)
        {
            PuzzleClear();
        }
    }

    public override void PuzzleClear()
    {
        _owner.StartPuzzleClear();

        Managers.UI.ClosePopupUI();
    }
}