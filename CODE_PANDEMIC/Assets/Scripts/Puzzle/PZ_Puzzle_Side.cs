using Inventory.Model;
using UnityEngine;

public abstract class PZ_Puzzle_Side : MonoBehaviour
{
    private ItemData _itemData;
    protected PuzzleData _data;

    public void SetInfo(PuzzleData data)
    {
        _data = data;
    }

    protected void RewardSetting(ItemData rewardData)
    {
        _itemData = rewardData;
    }

    protected void GiveRewardItem(int quantity)
    {
        Managers.Event.InvokeEvent("ItemReward", _itemData);

        Managers.Game.Inventory.AddItem(_itemData, quantity);
    }

    protected abstract void PuzzleClear();
}