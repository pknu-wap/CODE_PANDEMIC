using UnityEngine;

public class PZ_Key : PZ_Interact_Spawn
{
    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        Destroy(gameObject);
    }
}