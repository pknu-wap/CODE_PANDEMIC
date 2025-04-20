using UnityEngine;

public class PZ_Interact_Bed : MonoBehaviour, IInteractable
{
    private Transform _transform;

    private bool _isInteracted = false;

    // 하이라이트 기능
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _highlightMaterial;

    private void Start()
    {
        _transform = GetComponent<Transform>();

        _transform.localScale = new Vector3(2, 2, 1);
    }

    // 침대 상호 작용
    public void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        Debug.Log("숨겨진 아이템 획득");

        // 여기에 아이템 획득 기능 구현

        _isInteracted = true;
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