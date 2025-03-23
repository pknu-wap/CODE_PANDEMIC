using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic; // ���� ����

public class PZ_Sliding_Tile : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI _tileNumberText; // �׽�Ʈ�� ���� Ÿ�� ��ȣ ����� ���� text
    private int _tileNumber; // ���� Ÿ�� ��ȣ

    private PZ_Sliding_Board _slidingBoard;
    private Vector3 _correctPosition;
    public bool _isCorrect = false;

    private Image _image; // ���� �̹���

    // �ش� �κ��� �ӽ÷� �̷��� �� ���߿� resource �������� ���ҽ� �ε�� ���� �� ����
    [SerializeField]
    private List<Sprite> _puzzleSprites;

    // ���� ����
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
        _slidingBoard = GetComponentInParent<PZ_Sliding_Board>();
        _image = GetComponent<Image>();
    }

    // tileEmptyIndex ������ �� Ÿ���� index
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

    // ���� ��ġ ����
    public void SetCorrectPosition()
    {
        _correctPosition = GetComponent<RectTransform>().localPosition;
    }

    // Ŭ�� �̺�Ʈ
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click" + _tileNumber);

        _slidingBoard.MoveTile(this);
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
        _slidingBoard.IsPuzzleClear();
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
}