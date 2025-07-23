using System;
using Inventory.Model;

public abstract class PZ_Puzzle_UI_Side : PZ_Puzzle_UI_Base
{
    protected virtual void GiveReward(Action action = null)
    {
        if (Managers.Data.Puzzles.TryGetValue(_data.ID, out PuzzleData data))
        {
            RewardData reward = data.RewardItem;
            if (Managers.Data.Items.TryGetValue(_data.RewardItem.ID, out ItemData items))
            {
                Managers.Game.Inventory.AddItem(items, reward.Quantity);
                Managers.Event.InvokeEvent("ItemReward", items);
            }
        }
    }
}