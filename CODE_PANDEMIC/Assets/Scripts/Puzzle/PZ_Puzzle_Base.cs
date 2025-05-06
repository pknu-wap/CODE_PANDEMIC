using UnityEngine;
using UnityEngine.UI;

public abstract class PZ_Puzzle_Base : UI_PopUp
{
    protected PZ_Puzzle_Item _puzzleOwner; // 퍼즐 오너

    protected RectTransform _rectTransform;
    protected Image _image;

    public override bool Init()
    {
        return base.Init();
    }

    protected void SetComponents()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    // 퍼즐 오너 세팅
    public void SetPuzzleOwnerItem(PZ_Puzzle_Item owner)
    {
        _puzzleOwner = owner;
    }

    // 퍼즐 클리어
    protected abstract void PuzzleClear();
}