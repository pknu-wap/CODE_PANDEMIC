using UnityEngine;

public class PZ_HealPack : PZ_Interact_Spawn
{
    [SerializeField] private Sprite _interactedSprite;

    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        _spriteRenderer.sprite = _interactedSprite;
        Managers.Game.InteractedObjects(_interactData.ID);
        GiveReward();
    }
}