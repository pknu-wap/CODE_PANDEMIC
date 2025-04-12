using UnityEngine;

public class PZ_Interact_Bed : MonoBehaviour, IInteractable
{
    private RectTransform _rectTransform;

    private bool _isInteracted = false;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        _rectTransform.sizeDelta = new Vector3(2, 2, 1);
    }

    // 침대 상호 작용
    public void Interact()
    {
        if (_isInteracted)
        {
            return;
        }

        Debug.Log("숨겨진 아이템 획득");

        // 여기에 아이템 획득 기능 구현

        _isInteracted = true;
    }
}