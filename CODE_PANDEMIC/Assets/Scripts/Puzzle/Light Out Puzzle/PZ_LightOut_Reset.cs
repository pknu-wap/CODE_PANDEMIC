using UnityEngine;

public class PZ_LightOut_Reset : UI_Base
{
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        // ���� ��ư �⺻ ����
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = new Vector2(660, 0);
        _rectTransform.sizeDelta = new Vector2(200, 100);

        BindEvent(gameObject, OnButtonClick);
    }

    // ��ư Ŭ�� �̺�Ʈ
    private void OnButtonClick()
    {
        Debug.LogWarning("Reset!!!");

        Canvas canvas = GetComponentInParent<Canvas>();
        PZ_LightOut_Board board = canvas.GetComponentInChildren<PZ_LightOut_Board>();
        board.ResetButtons();
    }
}