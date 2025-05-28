using Inventory.Model;
using static Define;

public class PZ_Fish_Puzzle : PZ_Puzzle_Side
{
    public void ClearPuzzle()
    {
        PuzzleClear();
    }

    protected override void PuzzleClear()
    {

        GiveRewardItem();

        Destroy(gameObject);
    }
}