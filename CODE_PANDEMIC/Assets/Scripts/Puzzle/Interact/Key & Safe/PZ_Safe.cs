using UnityEngine;

public class PZ_Safe : MonoBehaviour, IInteractable
{
    private bool _isInteracted = false;

    public bool _hasKey = false; // 열쇠로 열기

    // 하이라이트 기능
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _lockMaterial;
    [SerializeField] private Material _unLockMaterial;

    public void Interact(GameObject player)
    {
        if (_isInteracted || !_hasKey)
        {
            Debug.Log("열쇠가 없음");
            return;
        }

        _isInteracted = true;
    }

    public void OnHighLight()
    {
        if (_hasKey)
        {
            _spriteRenderer.material = _unLockMaterial;
        }
        else
        {
            _spriteRenderer.material = _lockMaterial;
        }
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