using UnityEngine;
using System.Collections.Generic;

public class PZ_ArmorHouse : PZ_Interact_Spawn
{
    [SerializeField] List<int> needItemID = new List<int>();

    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        for (int index = 0; index < needItemID.Count; index++)
        {
            if (Managers.Game.Inventory.HasItem(needItemID[index]) == -1)
            {
                return;
            }
        }

        base.Interact(player);

        for (int index = 0; index < needItemID.Count; index++)
        {
            Managers.Game.Inventory.RemoveItem(needItemID[index], 1);
        }

        Managers.Game.InteractedObjects(_interactData.ID);
        RewardItem();
    }
}