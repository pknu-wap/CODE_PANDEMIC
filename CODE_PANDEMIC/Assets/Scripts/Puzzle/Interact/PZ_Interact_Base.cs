using UnityEngine;

public class PZ_Interact_Base : MonoBehaviour, IInteractable
{
    protected bool _isInteracted = false;

    // 하이라이트 기능
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Material _defaultMaterial;
    [SerializeField] protected Material _highlightMaterial;


    public virtual void Interact(GameObject player)
    {
        _isInteracted = true;
    }

    public virtual void OnHighLight()
    {
        _spriteRenderer.material = _highlightMaterial;
    }

    public virtual void OffHighLight()
    {
        _spriteRenderer.material = _defaultMaterial;
    }

    public bool IsInteractable()
    {
        return _isInteracted;
    }
}