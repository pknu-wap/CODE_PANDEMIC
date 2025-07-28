using UnityEngine;
using UnityEngine.UI;

public abstract class PZ_Puzzle_UI_Base : UI_PopUp, IPZ_Puzzle_Base
{
    #region Base

    protected RectTransform _rectTransform;
    protected Image _image;

    protected PZ_Puzzle_Item _puzzleOwner;
    protected PuzzleData _data;

    public override bool Init()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();

        return base.Init();
    }

    private void Start()
    {
        Managers.UI.SetCanvas(gameObject);

        ReadyToPause();
    }

    #endregion

    #region Setting

    public void SetPuzzleOwner(PZ_Puzzle_Item owner)
    {
        _puzzleOwner = owner;
    }

    public virtual void SetInfo(PuzzleData data)
    {
        _data = data;
    }

    #endregion

    #region Pause

    protected void ReadyToPause()
    {
        Managers.Game.PauseGame();
    }

    private void OnDisable()
    {
        Managers.Game.ResumeGame();
    }

    #endregion

    #region Clear

    public abstract void CheckPuzzleClear();

    public virtual void PuzzleClear()
    {
        _puzzleOwner.ClearPuzzle();

        Managers.UI.ClosePopupUI();
        Managers.Game.ClearPuzzle(_data.ID);
        Managers.Game.AddClearPuzzleCount();
    }

    #endregion
}