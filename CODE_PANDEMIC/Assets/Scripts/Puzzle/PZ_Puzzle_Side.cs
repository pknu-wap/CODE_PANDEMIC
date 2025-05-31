using System;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

public  class PZ_Puzzle_Side : MonoBehaviour
{
    protected PuzzleData _data;

    public void SetInfo(PuzzleData data)
    {
        _data = data;
    }

    protected void GiveRewardItem(Action action=null)
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

    protected virtual void PuzzleClear()
    {
        Managers.Game.ClearPuzzle(_data.ID);
    }
}