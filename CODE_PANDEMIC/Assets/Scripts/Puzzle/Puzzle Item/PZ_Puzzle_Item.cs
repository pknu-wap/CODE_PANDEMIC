using UnityEngine;

public class PZ_Puzzle_Item : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    private Canvas _canvas;

    [SerializeField]
    private string _puzzleAddressable; // ȭ�鿡 ����� ���� ��巹����

    private void Start()
    {
        if (_puzzleAddressable.Length == 0)
        {
            Debug.Log("���� �Ҵ� ����");
        }

        _boxCollider = GetComponent<BoxCollider2D>();

        _boxCollider.isTrigger = true;
        _boxCollider.offset = Vector2.zero;

        _canvas = FindObjectOfType<Canvas>();
    }

    // ���� ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        Managers.UI.ShowPopupUI<UI_PopUp>(_puzzleAddressable, _canvas.transform); // ���� ����
    }

    // ���� �ݱ�
    private void OnTriggerExit2D(Collider2D other)
    {
        Managers.UI.ClosePopupUI();
    }
}