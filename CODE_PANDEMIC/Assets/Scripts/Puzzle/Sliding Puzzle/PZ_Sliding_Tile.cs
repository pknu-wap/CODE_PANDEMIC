using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PZ_Sliding_Tile : UI_Base
{
    #region Base

    private TextMeshProUGUI _tileNumberText; // ���� Ÿ�� ��ȣ ����� ���� text
    private int _tileNumber; // ���� Ÿ�� ��ȣ

    private PZ_Sliding_Board _slidingBoard;
    private Vector3 _correctPosition; // �ùٸ� ��ġ�� Position
    public bool _isCorrect = false; // ���� Ÿ���� ��ġ�� �ùٸ� ��ġ���� �Ǵ�

    private Image _image; // ���� �̹���

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

    // tileEmptyIndex ������ �� Ÿ���� index
    public void TileSetup(int tileNumber, int tileEmptyIndex)
    {
        _slidingBoard = GetComponentInParent<PZ_Sliding_Board>();
        _image = GetComponent<Image>();
        _tileNumberText = GetComponentInChildren<TextMeshProUGUI>();

        TileNumber = tileNumber;

        // �����̵� ���� Ÿ�� Sprite �񵿱� �ε�
        string tileSpriteKey = "PZ_Sliding_Tile_" + tileNumber.ToString() + "_Sprite";
        Managers.Resource.LoadAsync<Sprite>(tileSpriteKey, (imageSprite) =>
        {
            _image.sprite = imageSprite;

            // �� Ÿ�� ��Ȱ��ȭ
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

    // Ÿ�� �ڿ������� �����̱�
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

        // Ÿ�� ��ġ üũ
        _isCorrect = CheckCorrectPosition();

        // ���� Ŭ���� üũ
        _slidingBoard.CheckPuzzleClear();
    }

    #endregion

    #region Check

    // ���� ��ġ ����
    public void SetCorrectPosition()
    {
        _correctPosition = GetComponent<RectTransform>().localPosition;
    }

    // ���� Ÿ�� ��ġ�� �ùٸ� ��ġ���� Ȯ��
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

    // Ŭ�� �̺�Ʈ
    public void OnTileClick()
    {
        Debug.Log("Click" + _tileNumber);

        _slidingBoard.MoveTile(this);
    }
}