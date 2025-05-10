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

        Debug.Log("열쇠 획득");

        // 여기에 열쇠 획득 기능 구현

        Destroy(gameObject);
    }
}