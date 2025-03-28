using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PZ_Sliding_Tile : MonoBehaviour, IPointerClickHandler
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

    public bool Init()
    {
        _slidingBoard = GetComponentInParent<PZ_Sliding_Board>();
        _image = GetComponent<Image>();

        if (!_slidingBoard || !_image)
        {
            return false;
        }

        return true;
    }

    #endregion

    #region Tile

    // tileEmptyIndex ������ �� Ÿ���� index
    public void TileSetup(int tileNumber, int tileEmptyIndex)
    {
        _tileNumberText = GetComponentInChildren<TextMeshProUGUI>();

        TileNumber = tileNumber;

        // �����̵� ���� Ÿ�� Sprite �񵿱� �ε�
        string tileSpriteKey = "PZ_Sliding_Tile_" + tileNumber.ToString();
        Managers.Resource.LoadAsync<Sprite>(tileSpriteKey, (imageSprite) =>
        {
            _image.sprite = imageSprite;
        });

        // �� Ÿ�� ��Ȱ��ȭ
        if (TileNumber == tileEmptyIndex)
        {
            GetComponent<Image>().enabled = false;
            _tileNumberText.enabled = false;
        }
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
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click" + _tileNumber);

        _slidingBoard.MoveTile(this);
    }
}