public class PZ_Soccer : PZ_Puzzle_Side
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