using UnityEngine;

public abstract class PZ_Puzzle_Interact_Base : MonoBehaviour, IPZ_Puzzle_Base
{
    protected PuzzleData _data;

    public virtual void SetInfo(PuzzleData data)
    {
        _data = data;
    }

    public abstract void CheckPuzzleClear();

    public virtual void PuzzleClear()
    {
        Managers.Game.ClearPuzzle(_data.ID);
        Managers.Game.AddClearPuzzleCount();
    }
}