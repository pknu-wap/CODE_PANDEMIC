using Inventory.Model;

public class PZ_Puzzle_Side : PZ_Puzzle_Base
{
    private ItemData _itemData;

    protected void RewardSetting(ItemData rewardData)
    {
        _itemData = rewardData;
    }

    protected void GiveRewardItem(int quantity)
    {
        Managers.Event.InvokeEvent("ItemReward", _itemData);

        Managers.Game.Inventory.AddItem(_itemData, quantity);
    }

    protected override void PuzzleClear()
    {

    }
}