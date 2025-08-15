using UnityEngine;

public class PZ_Catch_Target : UI_Base
{
    [SerializeField] private PZ_Catch_Board _board;

    private void Start()
    {
        BindEvent(gameObject, OnTargetClick, Define.UIEvent.Click);

        RandomPosition();
    }

    private void RandomPosition()
    {
        int posX = Random.Range(-550, 550);
        int posY = Random.Range(-300, 300);

        RectTransform rectTransform = GetComponent<RectTransform>();

        rectTransform.localPosition = new Vector3(posX, posY, 0);
    }

    public void OnTargetClick()
    {
        _board.IncreaseCount();

        Destroy(gameObject);
    }
}