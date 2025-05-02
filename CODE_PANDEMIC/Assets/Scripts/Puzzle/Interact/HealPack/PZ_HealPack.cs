using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

public class PZ_HealPack : PZ_Interact_Spawn
{
    [SerializeField] private Sprite _interactedSprite;

    // 벽 힐팩 상호 작용
    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        _spriteRenderer.sprite = _interactedSprite;

        RewardItem();
        // 여기에 아이템 획득 기능 구현
    }
    
       
}