using UnityEngine;

public class PZ_HealPack : MonoBehaviour, IInteractable
{
    private bool _isInteracted = false;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _interactedSprite;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
}