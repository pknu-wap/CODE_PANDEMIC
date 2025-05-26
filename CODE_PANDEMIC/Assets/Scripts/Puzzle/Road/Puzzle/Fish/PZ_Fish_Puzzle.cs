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
        ItemData rewardData = new ItemData();
        rewardData.TemplateID = 401;
        rewardData.Name = "오토바이 헬멧";
        rewardData.Description = "중국집 배달용 헬멧이다.\n방어력을 <color=#cbfc07>10</color>,체력을 <color=#cbfc07>20</color>을 올려준다";
        rewardData.IsStackable = false;
        rewardData.MaxStackSize = 1;
        rewardData.Sprite = "Armor_1";
        rewardData.Type = ItemType.Equippable;

        RewardSetting(rewardData);

        GiveRewardItem(1);

        Destroy(gameObject);
    }
}