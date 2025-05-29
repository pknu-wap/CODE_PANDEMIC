using UnityEngine;
using UnityEngine.UI;

public abstract class PZ_Puzzle_Base : UI_PopUp
{
    protected PZ_Puzzle_Item _puzzleOwner; // 퍼즐 오너

    protected RectTransform _rectTransform;
    protected Image _image;

    protected PuzzleData _data;

    public override bool Init()
    {
        return base.Init();
    }

    protected void SetComponents()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    public virtual void SetInfo(PuzzleData data)
    {
        _data = data;
    }

    // 퍼즐 오너 세팅
    public void SetPuzzleOwnerItem(PZ_Puzzle_Item owner)
    {
        _puzzleOwner = owner;
    }
    
    // 퍼즐 클리어
    protected abstract void PuzzleClear();

    protected void ReadyToPause()
    {
        Managers.Game.PauseGame();
    }

    private void OnDisable()
    {
        Managers.Game.ResumeGame();
    }
}