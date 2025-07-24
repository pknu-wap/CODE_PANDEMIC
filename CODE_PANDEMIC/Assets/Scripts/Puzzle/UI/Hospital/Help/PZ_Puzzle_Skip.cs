using UnityEngine;

public class PZ_Puzzle_Skip : UI_Base
{
    [SerializeField] private PZ_Puzzle_UI_Base _mainPuzzle;

    private void Start()
    {
        BindEvent(gameObject, OnButtonClick, Define.UIEvent.Click);
    }

    public void OnButtonClick()
    {
        _mainPuzzle.PuzzleClear();
    }
}