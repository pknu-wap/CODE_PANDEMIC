using UnityEngine;
using System.Collections;

public class PZ_Poster : MonoBehaviour, IInteractable
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

        _isInteracted = true;

        StartCoroutine(MoveDown());
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

    private IEnumerator MoveDown()
    {
        float currentTime = 0f;
        float currentPercent = 0f;
        float moveDuration = 1f;
        float targetPosY = transform.position.y - 1;

        while (currentPercent < 1)
        {
            currentTime += Time.deltaTime;
            currentPercent = currentTime / moveDuration;

            Vector3 tempPos = transform.position;
            tempPos.y = Mathf.Lerp(transform.position.y, targetPosY, currentTime);

            transform.position = tempPos;

            yield return null;
        }
    }
}