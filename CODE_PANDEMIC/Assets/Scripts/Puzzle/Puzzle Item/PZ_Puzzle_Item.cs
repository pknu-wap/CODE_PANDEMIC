using UnityEngine;
using System.Collections;

public class PZ_Puzzle_Item : MonoBehaviour, IInteractable
{
    #region Base

    private BoxCollider2D _boxCollider;
    private BoxCollider2D _blockObject; // 메인 퍼즐인 경우 지역을 막는 블럭
    private Canvas _canvas; // 테스트용

    private Vector3 _original; // 원래 위치
    private Vector3 _dongdong; // 둥둥 위치

    private PZ_Puzzle_Base _popupPuzzle; // 퍼즐

    [SerializeField]
    private string _puzzleAddressable; // 화면에 출력할 퍼즐 어드레서블
    [SerializeField]
    private bool _isMainPuzzle = true; // 메인 퍼즐인지 서브 퍼즐인지 체크

    private void Start()
    {
        if (_puzzleAddressable.Length == 0)
        {
            Debug.Log("퍼즐 할당 실패");
            return;
        }

        _boxCollider = GetComponent<BoxCollider2D>();
        _blockObject = GetComponentInChildren<BoxCollider2D>();

        _boxCollider.isTrigger = true;
        _boxCollider.offset = Vector2.zero;

        _canvas = FindObjectOfType<Canvas>(); // 삭제 예정
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

    // 퍼즐 띄우기
    public void Interact()
    {
        Debug.Log("퍼즐 상호 작용");

        // 캔버스 부분 수정 예정
        Managers.UI.ShowPopupUI<PZ_Puzzle_Base>(_puzzleAddressable, _canvas.transform, (popupPuzzle) =>
        {
            _popupPuzzle = popupPuzzle;
            _popupPuzzle.SetPuzzleOwnerItem(this);
        });
    }
    
    // 퍼즐 닫기
    public void ClosePuzzle()
    {
        Managers.UI.ClosePopupUI(_popupPuzzle);
    }

    #endregion

    #region Dong Dong

    // 위로 둥둥
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

    // 아래로 둥둥
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

        // 서브 퍼즐 클리어 시
        if(!_isMainPuzzle)
        {
            // 아이템 혹은 보상을 주는 로직 구현 예정
        }
    }

    #endregion
}