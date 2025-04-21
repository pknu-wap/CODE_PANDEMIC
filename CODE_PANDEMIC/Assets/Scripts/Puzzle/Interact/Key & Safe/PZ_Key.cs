using UnityEngine;

public class PZ_Key : MonoBehaviour, IInteractable
{
    private bool _isInteracted = false;

    // 하이라이트 기능
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _highlightMaterial;

    public void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        Debug.Log("열쇠 획득");

        // 여기에 열쇠 획득 기능 구현

        _isInteracted = true;

        Destroy(gameObject);
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