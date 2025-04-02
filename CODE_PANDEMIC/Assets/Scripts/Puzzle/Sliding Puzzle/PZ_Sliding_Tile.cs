using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PZ_Sliding_Tile : UI_Base
{
    #region Base

    private TextMeshProUGUI _tileNumberText; // 현재 타일 번호 출력을 위한 text
    private int _tileNumber; // 현재 타일 번호

    private PZ_Sliding_Board _slidingBoard;
    private Vector3 _correctPosition; // 올바른 위치의 Position
    public bool _isCorrect = false; // 현재 타일의 위치가 올바른 위치인지 판단

    private Image _image; // 퍼즐 이미지

    public int TileNumber
    {
        get { return _tileNumber; }

        set
        {
            _tileNumber = value;
            _tileNumberText.text = _tileNumber.ToString();
        }
    }

    #endregion

    #region Tile

    // tileEmptyIndex 마지막 빈 타일의 index
    public void TileSetup(int tileNumber, int tileEmptyIndex)
    {
        _slidingBoard = GetComponentInParent<PZ_Sliding_Board>();
        _image = GetComponent<Image>();
        _tileNumberText = GetComponentInChildren<TextMeshProUGUI>();

        TileNumber = tileNumber;

        // 슬라이딩 퍼즐 타일 Sprite 비동기 로딩
        string tileSpriteKey = "PZ_Sliding_Tile_" + tileNumber.ToString() + "_Sprite";
        Managers.Resource.LoadAsync<Sprite>(tileSpriteKey, (imageSprite) =>
        {
            _image.sprite = imageSprite;

            // 빈 타일 비활성화
            if (TileNumber == tileEmptyIndex)
            {
                _image.enabled = false;
                _tileNumberText.enabled = false;
            }
        });

        BindEvent(gameObject, OnTileClick);
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
        _slidingBoard.CheckPuzzleClear();
    }

    #endregion

    #region Check

    // 정답 위치 설정
    public void SetCorrectPosition()
    {
        _correctPosition = GetComponent<RectTransform>().localPosition;
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

    #endregion

    // 클릭 이벤트
    public void OnTileClick()
    {
        Debug.Log("Click" + _tileNumber);

        _slidingBoard.MoveTile(this);
    }
}