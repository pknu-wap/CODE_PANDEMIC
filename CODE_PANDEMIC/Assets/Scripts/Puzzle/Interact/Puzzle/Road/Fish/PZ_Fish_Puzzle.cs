public class PZ_Fish_Puzzle : PZ_Puzzle_Interact_Side
{
    public override void CheckPuzzleClear()
    {
        PuzzleClear();
    }

    public override void PuzzleClear()
    {
        base.PuzzleClear();

        Destroy(gameObject);
    }
}