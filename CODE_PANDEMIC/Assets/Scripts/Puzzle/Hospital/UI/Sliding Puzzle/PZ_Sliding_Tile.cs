using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PZ_Sliding_Tile : UI_Base
{
    #region Base

    private TextMeshProUGUI _tileNumberText;
    private int _tileNumber;

    private PZ_Sliding_Board _slidingBoard;
    private Vector3 _correctPosition;
    public bool _isCorrect = false;

    private Image _image;

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

    public void TileSetup(int tileNumber, int tileEmptyIndex)
    {
        _slidingBoard = GetComponentInParent<PZ_Sliding_Board>();
        _image = GetComponent<Image>();
        _tileNumberText = GetComponentInChildren<TextMeshProUGUI>();

        TileNumber = tileNumber;

        string tileSpriteKey = "PZ_Sliding_Tile_" + tileNumber.ToString() + "_Sprite";
        Managers.Resource.LoadAsync<Sprite>(tileSpriteKey, (imageSprite) =>
        {
            _image.sprite = imageSprite;

            if (TileNumber == tileEmptyIndex)
            {
                _image.enabled = false;
                _tileNumberText.enabled = false;
            }
        });

        BindEvent(gameObject, OnTileClick, Define.UIEvent.Click);
    }

    public void TileMoveto(Vector3 endPosition)
    {
        StartCoroutine("TileMove", endPosition);
    }

    private IEnumerator TileMove(Vector3 endPosition)
    {
        float currentTime = 0f;
        float currentPecentage = 0f;
        float moveDuration = 0.1f;
        Vector3 startPosition = GetComponent<RectTransform>().localPosition;

        while (currentPecentage < 1)
        {
            currentTime += Time.unscaledDeltaTime;
            currentPecentage = currentTime / moveDuration;
            GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPosition, endPosition, currentPecentage);

            yield return null;
        }

        _isCorrect = CheckCorrectPosition();

        _slidingBoard.CheckPuzzleClear();
    }

    #endregion

    #region Check

    public void SetCorrectPosition()
    {
        _correctPosition = GetComponent<RectTransform>().localPosition;
    }

    private bool CheckCorrectPosition()
    {
        if (GetComponent<RectTransform>().localPosition == _correctPosition)
        {
            return true;
        }

        return false;
    }

    #endregion

    #region Event

    public void OnTileClick()
    {
        _slidingBoard.MoveTile(this);
    }

    #endregion
}