using UnityEngine;

public class PZ_Interact_Bed : PZ_Interact_Base
{
    // 침대 상호 작용
    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        Debug.Log("숨겨진 아이템 획득");

        // 여기에 아이템 획득 기능 구현
    }
}