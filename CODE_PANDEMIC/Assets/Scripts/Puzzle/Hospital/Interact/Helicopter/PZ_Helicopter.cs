using UnityEngine;

public class PZ_Helicopter : PZ_Interact_NonSpawn
{
    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        Managers.Event.InvokeEvent("NextStage");
    }
}