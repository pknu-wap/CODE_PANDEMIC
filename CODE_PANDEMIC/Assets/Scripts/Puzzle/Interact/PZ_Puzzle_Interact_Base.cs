using System;
using UnityEngine;
using Inventory.Model;

public abstract class PZ_Puzzle_Interact_Base : MonoBehaviour, IPZ_Puzzle_Base
{
    protected PuzzleData _data;

    public virtual void SetInfo(PuzzleData data)
    {
        _data = data;
    }

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

    public abstract void CheckPuzzleClear();

    public virtual void PuzzleClear()
    {
        Managers.Game.ClearPuzzle(_data.ID);
    }
}