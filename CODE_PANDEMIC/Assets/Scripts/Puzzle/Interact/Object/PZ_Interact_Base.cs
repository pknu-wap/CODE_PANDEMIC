using UnityEngine;
using System.Collections;

public class PZ_Interact_Base : MonoBehaviour, IInteractable
{
    protected bool _isInteracted = false;

    private Coroutine _highLight;

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

        if (_highLight != null)
        {
            return;
        }

        _highLight = StartCoroutine(AutoOffHighLight());
    }

    public virtual void OffHighLight()
    {
        if (_highLight != null)
        {
            StopCoroutine(_highLight);
            _highLight = null;
        }

        _spriteRenderer.material = _defaultMaterial;
    }

    public bool IsInteractable()
    {
        return _isInteracted;
    }

    private IEnumerator AutoOffHighLight()
    {
        yield return CoroutineHelper.WaitForSeconds(1.0f);

        _spriteRenderer.material = _defaultMaterial;
        _highLight = null;
    }
}