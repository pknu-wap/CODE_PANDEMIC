using UnityEngine;
using System.Collections;
using System;

public class PZ_Fish_Sign : PZ_Interact_NonSpawn
{
    [SerializeField] private GameObject _timeUI;

    public static event Action StartFishFind;

    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        Managers.UI.ShowPopupUI<PZ_Fish_UI>("PZ_Fish_UI_Prefab", null, (getUI) =>
        {
            StartCoroutine(CloseMainText(getUI));
        });

        StartFishFind?.Invoke();
    }

    private IEnumerator CloseMainText(PZ_Fish_UI popupUI)
    {
        yield return CoroutineHelper.WaitForSeconds(5.0f);

        popupUI.CloseMainText();
        StartCoroutine(popupUI.FishFindTimer());
    }
}