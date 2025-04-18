using UnityEngine;

public enum ElevatingMap
{
    Hospital3,
    Hospital2,
    Hospital1
}

public class PZ_Elevator : MonoBehaviour, IInteractable
{
    [SerializeField] private ElevatingMap _elevatingMap; // 해당 엘레베이터로 이동할 맵
    private Transform _transform;
    private Animator _animator;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();

        _transform.localScale = new Vector3(1.5f, 1.5f, 1);
    }

    // 엘레베이터 상호 작용
    public void Interact()
    {
        _animator.SetBool("IsOpened", true);

        // 여기에 다음 맵으로 넘어가는 기능 구현
        switch(_elevatingMap)
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
    }
}