public class PZ_Fish_Puzzle : PZ_Puzzle_Side
{
    public void ClearPuzzle()
    {
        PuzzleClear();
    }

    protected override void PuzzleClear()
    {
        base.PuzzleClear();
        GiveRewardItem();

        Destroy(gameObject);
    }
}