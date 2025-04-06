using UnityEngine;

public class PZ_Puzzle_Item : MonoBehaviour
{
    #region Base

    private BoxCollider2D _boxCollider;
    private BoxCollider2D _blockObject; // ���� ������ ��� ������ ���� ��
    private Canvas _canvas; // �׽�Ʈ��

    private PZ_Puzzle_Base _popupPuzzle; // ����

    [SerializeField]
    private string _puzzleAddressable; // ȭ�鿡 ����� ���� ��巹����
    [SerializeField]
    private bool _isMainPuzzle = true; // ���� �������� ���� �������� üũ

    private void Start()
    {
        if (_puzzleAddressable.Length == 0)
        {
            Debug.Log("���� �Ҵ� ����");
            return;
        }

        _boxCollider = GetComponent<BoxCollider2D>();
        _blockObject = GetComponentInChildren<BoxCollider2D>();

        _boxCollider.isTrigger = true;
        _boxCollider.offset = Vector2.zero;

        _canvas = FindObjectOfType<Canvas>(); // ���� ����

        if (!_isMainPuzzle)
        {
            _blockObject.isTrigger = true;
        }
    }

    #endregion

    #region Trigger

    // ���� ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        Managers.UI.ShowPopupUI<PZ_Puzzle_Base>(_puzzleAddressable, _canvas.transform, (popupPuzzle) =>
        {
            _popupPuzzle = popupPuzzle;
            _popupPuzzle.SetPuzzleOwnerItem(this);
        });
    }

    // ���� �ݱ�
    private void OnTriggerExit2D(Collider2D other)
    {
        Managers.UI.ClosePopupUI(_popupPuzzle);
    }

    #endregion

    #region Clear

    public void ClearPuzzle()
    {
        Managers.UI.ClosePopupUI();

        // ���� ���� Ŭ���� �� ���� ���������� �� �� �ְ� ���� ������
        if (_isMainPuzzle)
        {
            Destroy(gameObject);
        }

        // ���� ���� Ŭ���� ��
        else
        {
            // ������ Ȥ�� ������ �ִ� ���� ���� ����
        }
    }

    #endregion
}