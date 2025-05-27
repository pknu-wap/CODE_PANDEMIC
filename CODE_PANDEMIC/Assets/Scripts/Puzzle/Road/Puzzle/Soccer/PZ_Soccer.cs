using Inventory.Model;
using static Define;

public class PZ_Soccer : PZ_Puzzle_Side
{
    public void ClearPuzzle()
    {
        PuzzleClear();
    }

    protected override void PuzzleClear()
    {
        ItemData rewardData = new ItemData();
        rewardData.TemplateID = 601;
        rewardData.Name = "선수용 러닝화";
        rewardData.Description = "신어도 안 신은것과 같이 가벼운느낌이 든다.\n이동속도를<color=#cbfc07>0.3</color> 올려준다.";
        rewardData.IsStackable = false;
        rewardData.MaxStackSize = 1;
        rewardData.Sprite = "Armor_3";
        rewardData.Type = ItemType.Equippable;

        RewardSetting(rewardData);

        GiveRewardItem(1);

        Destroy(gameObject);
    }
}