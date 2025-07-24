using System.Collections;
using UnityEngine;

public enum ElevatingMap
{
    Hospital3,
    Hospital2,
    Hospital1
}

public class PZ_Elevator : PZ_Interact_NonSpawn
{
    [SerializeField] private ElevatingMap _currentMap; // 현재 맵
    [SerializeField] private ElevatingMap _elevatingMap; // 해당 엘레베이터로 이동할 맵
    private Animator _animator;
    private PZ_Elevator_Screen _elevatorScreen; // 층수 화면
    [SerializeField] private bool _isNextElevator = true;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _elevatorScreen = GetComponentInChildren<PZ_Elevator_Screen>();

        _elevatorScreen.Setting(_currentMap);
    }

    // 엘레베이터 상호 작용
    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        StartCoroutine(ChangeStage());

        _animator.SetBool("IsOpened", true);

        _elevatorScreen.Setting(_elevatingMap);
    }

    private IEnumerator ChangeStage()
    {
        yield return new WaitForSeconds(1f);

        if (_isNextElevator)
        {
            Managers.Event.InvokeEvent("NextStage");
        }
        else
        {
            Managers.Event.InvokeEvent("PrevStage");
        }
    }
}