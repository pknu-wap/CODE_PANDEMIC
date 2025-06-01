using UnityEngine;

public class PZ_Fish : PZ_Interact_NonSpawn
{
    [SerializeField] private PZ_Fish_Puzzle _fishPuzzle;

    private bool _isDelayed = true;

    private void Start()
    {
        PZ_Fish_Sign.StartFishFind += StartFishFind;
        PZ_Fish_UI.FishTimeOver += FishTimeOver;
    }

    private void OnEnable()
    {
        Managers.Event.Subscribe("OnPlayerDead", PlayerDie);
    }

    private void OnDisable()
    {
        Managers.Event.Unsubscribe("OnPlayerDead", PlayerDie);
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

        FishTimeOver();

        _fishPuzzle.ClearPuzzle();
    }

    private void FishTimeOver()
    {
        _isDelayed = true;
        _isInteracted = true;

        Managers.UI.ClosePopupUI();
    }

    private void PlayerDie(object obj)
    {
        Managers.UI.ClosePopupUI();
    }
}