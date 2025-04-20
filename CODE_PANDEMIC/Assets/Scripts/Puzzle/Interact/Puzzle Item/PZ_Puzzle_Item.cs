using UnityEngine;
using System.Collections;

public class PZ_Puzzle_Item : MonoBehaviour, IInteractable
{
    #region Base

    private BoxCollider2D _boxCollider;

    private Vector3 _original; // 원래 위치
    private Vector3 _dongdong; // 둥둥 위치

    private PZ_Puzzle_Base _popupPuzzle; // 퍼즐

    private PZ_Main_Block _mainBlock; // 길 막는 오브젝트

    private string _puzzleAddressable; // 화면에 출력할 퍼즐 어드레서블
    private bool _isMainPuzzle = true; // 메인 퍼즐인지 서브 퍼즐인지 체크

    private bool _isInteracted = false;

    // 하이라이트 기능
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _highlightMaterial;

    public void SetInfo(PuzzleData data)
    {
        _puzzleAddressable = data.UIPath;
        _isMainPuzzle = data.IsMain;
    }

    public void SettingPuzzle()
    {
        _boxCollider = Utils.GetOrAddComponent<BoxCollider2D>(gameObject);

        _boxCollider.isTrigger = true;
        _boxCollider.offset = Vector2.zero;

        Managers.Object.RegisterPuzzles();

        _original = transform.position;
        _dongdong = _original;
        _dongdong.y += 0.2f;

        Managers.Resource.Instantiate("PZ_Main_Block_Prefab", null, (mainBlockObject) =>
        {
            _mainBlock = mainBlockObject.GetComponent<PZ_Main_Block>();

            // 여기에 데이터 넣어주삼
        });

        StartCoroutine(MoveUp());
    }

    #endregion

    #region Interact

    // 퍼즐 띄우기
    public void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        Debug.Log("퍼즐 상호 작용");

        _isInteracted = true;

        Managers.UI.ShowPopupUI<PZ_Puzzle_Base>(_puzzleAddressable, null, (popupPuzzle) =>
        {
            _popupPuzzle = popupPuzzle;
            _popupPuzzle.SetPuzzleOwnerItem(this);
        });
    }

    public void OnHighLight()
    {
        _spriteRenderer.material = _highlightMaterial;
    }

    public void OffHighLight()
    {
        _spriteRenderer.material = _defaultMaterial;
    }

    public bool IsInteractable()
    {
        return _isInteracted;
    }

    // 퍼즐 닫기, ESC를 눌렀을 때 이 함수를 호출해야 함
    public void ClosePuzzle()
    {
        _isInteracted = false;

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
        if (!_isMainPuzzle)
        {
            // 아이템 혹은 보상을 주는 로직 구현 예정
        }

        Destroy(_mainBlock.gameObject);
        Destroy(gameObject);
    }

    #endregion
}