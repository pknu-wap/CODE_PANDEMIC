using UnityEngine;

public class PZ_HealPack : MonoBehaviour, IInteractable
{
    private bool _isInteracted = false;
    [SerializeField] private Sprite _interactedSprite;

    // 하이라이트 기능
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _highlightMaterial;

    // 벽 힐팩 상호 작용
    public void Interact()
    {
        if (_isInteracted)
        {
            return;
        }

        _spriteRenderer.sprite = _interactedSprite;

        Debug.Log("회복 아이템 획득");

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