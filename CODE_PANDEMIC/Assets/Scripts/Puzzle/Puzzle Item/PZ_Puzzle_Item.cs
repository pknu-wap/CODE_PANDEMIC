using UnityEngine;
using System.Collections;

public class PZ_Puzzle_Item : MonoBehaviour, IInteractable
{
    #region Base

    private BoxCollider2D _boxCollider;
    private BoxCollider2D _blockObject; // ���� ������ ��� ������ ���� ��
    private Canvas _canvas; // �׽�Ʈ��

    private Vector3 _original; // ���� ��ġ
    private Vector3 _dongdong; // �յ� ��ġ

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
        Managers.Object.RegisterPuzzles();
        if (!_isMainPuzzle)
        {
            _blockObject.isTrigger = true;
        }

        _original = transform.position;
        _dongdong = _original;
        _dongdong.y += 0.2f;

        StartCoroutine(MoveUp());
    }

    #endregion

    #region Interact

    // ���� ����
    public void Interact()
    {
        Debug.Log("���� ��ȣ �ۿ�");

        // ĵ���� �κ� ���� ����
        Managers.UI.ShowPopupUI<PZ_Puzzle_Base>(_puzzleAddressable, _canvas.transform, (popupPuzzle) =>
        {
            _popupPuzzle = popupPuzzle;
            _popupPuzzle.SetPuzzleOwnerItem(this);
        });
    }
    
    // ���� �ݱ�
    public void ClosePuzzle()
    {
        Managers.UI.ClosePopupUI(_popupPuzzle);
    }

    #endregion

    #region Dong Dong

    // ���� �յ�
    private IEnumerator MoveUp()
    {
        float currentTime = 0f;
        float currentPecentage = 0f;
        float moveDuration = 1f;

        while (currentPecentage < 1)
        {
            currentTime += Time.deltaTime;
            currentPecentage = currentTime / moveDuration;

            transform.position = Vector3.Lerp(_original, _dongdong, currentPecentage);

            yield return null;
        }

        StartCoroutine(MoveDown());
    }

    // �Ʒ��� �յ�
    private IEnumerator MoveDown()
    {
        float currentTime = 0f;
        float currentPecentage = 0f;
        float moveDuration = 3f;

        while (currentPecentage < 1)
        {
            currentTime += Time.deltaTime;
            currentPecentage = currentTime / moveDuration;

            transform.position = Vector3.Lerp(_dongdong, _original, currentPecentage);

            yield return null;
        }

        StartCoroutine(MoveUp());
    }

    #endregion

    #region Clear

    public void ClearPuzzle()
    {
        ClosePuzzle();

        Managers.Object.UnRegisterPuzzles();

        // ���� ���� Ŭ���� ��
        if(!_isMainPuzzle)
        {
            // ������ Ȥ�� ������ �ִ� ���� ���� ����
        }
    }

    #endregion
}