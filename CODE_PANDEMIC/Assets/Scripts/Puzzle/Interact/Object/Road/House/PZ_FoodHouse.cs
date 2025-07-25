using UnityEngine;

public class PZ_FoodHouse : PZ_Interact_Spawn
{
    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        Managers.Game.InteractedObjects(_interactData.ID);
        GiveReward();
    }
}