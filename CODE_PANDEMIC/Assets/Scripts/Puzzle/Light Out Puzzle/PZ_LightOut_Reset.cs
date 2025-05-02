public class PZ_LightOut_Reset : UI_PopUp
{
    private void Start()
    {
        BindEvent(gameObject, OnButtonClick);
    }

    // 버튼 클릭 이벤트
    private void OnButtonClick()
    {
        PZ_LightOut_Board board = GetComponentInParent<PZ_LightOut_Board>();
        board.ResetButtons();
    }
}