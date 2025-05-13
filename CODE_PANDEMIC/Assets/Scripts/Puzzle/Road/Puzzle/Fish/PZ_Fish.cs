using UnityEngine;

public class PZ_Fish : PZ_Interact_NonSpawn
{
    private bool _isDelayed = true;

    private void Start()
    {
        PZ_Fish_Sign.StartFishFind += StartFishFind;
        PZ_Fish_UI.FishTimeOver += FishTimeOver;
    }

    private void StartFishFind()
    {
        _isDelayed = false;
    }

    public override void Interact(GameObject player)
    {
        if (_isDelayed || _isInteracted)
        {
            return;
        }

        Debug.Log("Fish Clear!!!");

        FishTimeOver();

        // 여기에 보상이나 무언가가 되는 로직 구현 예정
    }

    private void FishTimeOver()
    {
        _isDelayed = true;
        _isInteracted = true;

        Managers.UI.ClosePopupUI();
    }
}