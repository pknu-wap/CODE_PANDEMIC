using UnityEngine;

public class PZ_Interact_Bed : PZ_Interact_Spawn
{
    // 침대 상호 작용
    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        RewardItem();
    }
}