using UnityEngine;

public class PZ_RoadHelp : PZ_Interact_NonSpawn
{
    [SerializeField] private string _helpMessage;

    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        Managers.UI.ShowPopupUI<PZ_RoadHelp_Popup>("PZ_RoadHelp_Popup_Prefab", null, (getUI) =>
        {
            getUI.SetHelpText(this, _helpMessage);
        });
    }

    public void SetInteractable()
    {
        _isInteracted = false;
    }
}