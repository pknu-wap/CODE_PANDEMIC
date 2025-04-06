using UnityEngine;
using UnityEngine.UI;

public abstract class PZ_Puzzle_Base : UI_PopUp
{
    protected PZ_Puzzle_Item _puzzleOwner; // ���� ����

    protected RectTransform _rectTransform; // ���� Bind ���� ���� ����
    protected Image _image; // ���� Bind ���� ���� ����

    protected void SetComponents()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    // ���� ���� ����
    public void SetPuzzleOwnerItem(PZ_Puzzle_Item owner)
    {
        _puzzleOwner = owner;
    }

    // ���� Ŭ����
    protected abstract void PuzzleClear();
}