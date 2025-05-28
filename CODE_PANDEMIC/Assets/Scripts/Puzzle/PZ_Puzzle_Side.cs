using System.Collections.Generic;
using Inventory.Model;

public class PZ_Puzzle_Side : PZ_Puzzle_Base
{

    protected void GiveRewardItem()
    {
        if(Managers.Data.Puzzles.TryGetValue(_data.ID,out PuzzleData data))
        {
            RewardData reward = data.RewardItem;
            if (Managers.Data.Items.TryGetValue(_data.ID, out ItemData items))
            {
                Managers.Game.Inventory.AddItem(items, reward.Quantity);
            }

        }
       
    }

    protected override void PuzzleClear()
    {

    }
}