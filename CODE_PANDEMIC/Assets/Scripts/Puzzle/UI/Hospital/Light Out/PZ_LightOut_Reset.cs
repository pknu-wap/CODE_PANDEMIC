public class PZ_LightOut_Reset : UI_PopUp
{
    private void Start()
    {
        BindEvent(gameObject, OnButtonClick);
    }

    private void OnButtonClick()
    {
        PZ_LightOut_Board board = GetComponentInParent<PZ_LightOut_Board>();
        board.ResetButtons();
    }
}