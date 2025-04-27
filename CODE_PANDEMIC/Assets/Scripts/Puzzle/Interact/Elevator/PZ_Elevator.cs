using UnityEngine;

public enum ElevatingMap
{
    Hospital3,
    Hospital2,
    Hospital1
}

public class PZ_Elevator : PZ_Interact_Base
{
    [SerializeField] private ElevatingMap _currentMap; // 현재 맵
    [SerializeField] private ElevatingMap _elevatingMap; // 해당 엘레베이터로 이동할 맵
    private Animator _animator;
    private PZ_Elevator_Screen _elevatorScreen; // 층수 화면

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

        _animator.SetBool("IsOpened", true);

        // 여기에 다음 맵으로 넘어가는 기능 구현
        switch (_elevatingMap)
        {
            case ElevatingMap.Hospital3:
                Debug.Log("3층 이동");
                break;

            case ElevatingMap.Hospital2:
                Debug.Log("2층 이동");
                break;

            case ElevatingMap.Hospital1:
                Debug.Log("1층 이동");
                break;
        }

        _elevatorScreen.Setting(_elevatingMap);
    }
}