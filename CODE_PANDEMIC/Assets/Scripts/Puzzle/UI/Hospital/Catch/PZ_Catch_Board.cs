using UnityEngine;

public class PZ_Catch_Board : PZ_Puzzle_UI_Main
{
    private int _targetCount = 3;
    [SerializeField] private int _currentCount = 0;

    public void IncreaseCount()
    {
        _currentCount++;

        CheckPuzzleClear();
    }

    public override void CheckPuzzleClear()
    {
        if(_currentCount == _targetCount)
        {
            PuzzleClear();
        }
    }
}