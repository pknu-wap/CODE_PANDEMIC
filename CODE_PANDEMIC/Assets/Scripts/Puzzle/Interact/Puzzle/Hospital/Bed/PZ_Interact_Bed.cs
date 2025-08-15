using UnityEngine;

public class PZ_Interact_Bed : PZ_Puzzle_Item
{
    public override void SetInfo(PuzzleData data)
    {
        base.SetInfo(data);

        _activeDongDong = false;
    }
}