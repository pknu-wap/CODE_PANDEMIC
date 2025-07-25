public class PZ_Soccer : PZ_Puzzle_Interact_Side
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