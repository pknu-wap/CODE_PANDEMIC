using UnityEngine;

public class PZ_Puzzle_Skip : UI_Base
{
    [SerializeField] private PZ_Puzzle_Base _mainPuzzle;

    private void Start()
    {
        BindEvent(gameObject, OnButtonClick);
    }

    public void OnButtonClick()
    {
        _mainPuzzle.PuzzleClear();
    }
}