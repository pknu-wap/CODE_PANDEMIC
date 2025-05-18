using UnityEngine;

public class PZ_Fish_Puzzle : PZ_Puzzle_Side
{
    public void ClearPuzzle()
    {
        PuzzleClear();
    }

    protected override void PuzzleClear()
    {
        // 여기에 보상

        Destroy(gameObject);
    }
}