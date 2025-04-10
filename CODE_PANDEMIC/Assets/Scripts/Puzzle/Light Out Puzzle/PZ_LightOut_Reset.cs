using UnityEngine;

public class PZ_LightOut_Reset : UI_Base
{
    private RectTransform _rectTransform; // 추후 Bind 배우고 수정 예정

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        // 리셋 버튼 기본 세팅
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = new Vector2(660, 0);
        _rectTransform.sizeDelta = new Vector2(200, 100);

        BindEvent(gameObject, OnButtonClick);
    }

    // 버튼 클릭 이벤트
    private void OnButtonClick()
    {
        Debug.LogWarning("Reset!!!");

        Canvas canvas = GetComponentInParent<Canvas>();
        PZ_LightOut_Board board = canvas.GetComponentInChildren<PZ_LightOut_Board>();
        board.ResetButtons();
    }
}