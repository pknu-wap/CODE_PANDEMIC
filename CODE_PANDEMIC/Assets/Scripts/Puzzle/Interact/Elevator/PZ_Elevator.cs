using UnityEngine;

public enum ElevatingMap
{
    Hospital3,
    Hospital2,
    Hospital1
}

public class PZ_Elevator : MonoBehaviour, IInteractable
{
    [SerializeField] private ElevatingMap _currentMap; // 현재 맵
    [SerializeField] private ElevatingMap _elevatingMap; // 해당 엘레베이터로 이동할 맵
    private Transform _transform;
    private Animator _animator;

    private bool _isInteracted = false;

    // 하이라이트 기능
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _highlightMaterial;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();

        _transform.localScale = new Vector3(1.5f, 1.5f, 1);
    }

    // 엘레베이터 상호 작용
    public void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        _isInteracted = true;

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
    }

    public void OnHighLight()
    {
        _spriteRenderer.material = _highlightMaterial;
    }

    public void OffHighLight()
    {
        _spriteRenderer.material = _defaultMaterial;
    }

    public bool IsInteractable()
    {
        return _isInteracted;
    }
}