using UnityEngine;

public class PZ_Elevator : MonoBehaviour, IInteractable
{
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform=GetComponent<RectTransform>();

        _rectTransform.localScale = new Vector3(1.5f, 1.5f, 1);
    }

    // 엘레베이터 상호 작용
    public void Interact()
    {
        Debug.Log("엘레베이터 상호 작용");
        
        // 여기에 다음 맵으로 넘어가는 기능 구현
    }
}