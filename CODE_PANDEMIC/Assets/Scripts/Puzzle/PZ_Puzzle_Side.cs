using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

public abstract class PZ_Puzzle_Side : MonoBehaviour
{
    protected PuzzleData _data;

    public void SetInfo(PuzzleData data)
    {
        _data = data;
    }

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

    protected abstract void PuzzleClear();
}