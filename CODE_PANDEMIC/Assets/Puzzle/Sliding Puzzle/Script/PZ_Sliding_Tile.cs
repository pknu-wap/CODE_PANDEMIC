using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic; // 삭제 예정

public class PZ_Sliding_Tile : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI _tileNumberText; // 테스트용 현재 타일 번호 출력을 위한 text
    private int _tileNumber; // 현재 타일 번호

    private PZ_Sliding_Board _sliding_Board;
    private Vector3 _correctPosition;
    public bool _isCorrect = false;

    private Image _image; // 퍼즐 이미지

    // 해당 부분은 임시로 이렇게 함 나중에 resource 폴더에서 리소스 로드로 구현 할 예정
    [SerializeField]
    private List<Sprite> _puzzleSprites;

    // 삭제 예정
    public int TileNumber
    {
        get { return _tileNumber; }

        set
        {
            _tileNumber = value;
            _tileNumberText.text = _tileNumber.ToString();
        }
    }
    private void Awake()
    {
        _sliding_Board = GetComponentInParent<PZ_Sliding_Board>();
        _image = GetComponent<Image>();
    }

    // tileEmptyIndex 마지막 빈 타일의 index
    public void TileSetup(int tileNumber, int tileEmptyIndex)
    {
        _tileNumberText = GetComponentInChildren<TextMeshProUGUI>();
        TileNumber = tileNumber;
        _image.sprite = _puzzleSprites[tileNumber - 1];

        if (TileNumber == tileEmptyIndex)
        {
            GetComponent<Image>().enabled = false;
            _tileNumberText.enabled = false;
        }
    }

    // 정답 위치 설정
    public void SetCorrectPosition()
    {
        _correctPosition = GetComponent<RectTransform>().localPosition;
    }

    // 클릭 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click" + _tileNumber);

        _sliding_Board.MoveTile(this);
    }

    public void TileMoveto(Vector3 endPosition)
    {
        StartCoroutine("TileMove", endPosition);
    }

    // 타일 자연스럽게 움직이기
    private IEnumerator TileMove(Vector3 endPosition)
    {
        float currentTime = 0f;
        float currentPecentage = 0f;
        float moveDuration = 0.1f;
        Vector3 startPosition = GetComponent<RectTransform>().localPosition;

        while (currentPecentage < 1)
        {
            currentTime += Time.deltaTime;
            currentPecentage = currentTime / moveDuration;
            GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPosition, endPosition, currentPecentage);

            yield return null;
        }

        // 타일 위치 체크
        _isCorrect = CheckCorrectPosition();

        // 퍼즐 클리어 체크
        _sliding_Board.IsPuzzleClear();
    }

    // 현재 타일 위치가 올바른 위치인지 확인
    private bool CheckCorrectPosition()
    {
        if (GetComponent<RectTransform>().localPosition == _correctPosition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}