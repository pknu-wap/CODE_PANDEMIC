using UnityEngine;

public class PZ_BloodPack_Hanger : PZ_Interact_Base
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

        Managers.Resource.Instantiate("PZ_BloodPack_Prefab", null, (getObject) =>
        {
            getObject.GetComponent<PZ_BloodPack>().Setting(player.GetComponent<PlayerController>());
        });
    }
}