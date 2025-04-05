using UnityEngine;

public class PZ_Puzzle_Item : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    private Canvas _canvas;

    [SerializeField]
    private string _puzzleAddressable; // 화면에 출력할 퍼즐 어드레서블

    private void Start()
    {
        if (_puzzleAddressable.Length == 0)
        {
            Debug.Log("퍼즐 할당 실패");
        }

        _boxCollider = GetComponent<BoxCollider2D>();

        _boxCollider.isTrigger = true;
        _boxCollider.offset = Vector2.zero;

        _canvas = FindObjectOfType<Canvas>();
    }

    // 퍼즐 띄우기
    private void OnTriggerEnter2D(Collider2D other)
    {
        Managers.UI.ShowPopupUI<UI_PopUp>(_puzzleAddressable, _canvas.transform); // 수정 예정
    }

    // 퍼즐 닫기
    private void OnTriggerExit2D(Collider2D other)
    {
        Managers.UI.ClosePopupUI();
    }
}