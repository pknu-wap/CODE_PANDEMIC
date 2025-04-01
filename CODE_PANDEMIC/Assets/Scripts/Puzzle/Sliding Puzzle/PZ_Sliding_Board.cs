using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PZ_Sliding_Board : UI_PopUp
{
    #region Base

    private RectTransform _rectTransform; // ũ�� ������ ���� ������
    private List<PZ_Sliding_Tile> _tileList = new List<PZ_Sliding_Tile>(); // ������ Ÿ���� ����
    private int _slidingPuzzleSize = 9; // ���� ������
    private float _needMoveDistance = 232f; // Ÿ���� �̵��ؾ� �ϴ� �Ÿ�

    public Vector3 EmptyTilePosition { get; set; } // �� Ÿ���� ��ġ

    private IEnumerator Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        // ���� �⺻ ����
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = Vector2.zero;
        _rectTransform.sizeDelta = new Vector2(730, 730);

        GetSpawnedTiles();

        // ��ġ ���� ���� ����ȭ
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());

        yield return new WaitForEndOfFrame();

        // ���� ��ġ�� ���� ��ġ�� ����
        _tileList.ForEach(tile => tile.SetCorrectPosition());

        StartCoroutine("ShuffleTiles");
    }

    #endregion

    #region Tile

    private void GetSpawnedTiles()
    {
        for (int index = 0; index < _slidingPuzzleSize; index++)
        {
            Transform childTile = transform.GetChild(index);
            PZ_Sliding_Tile spawnedTile = childTile.gameObject.GetComponent<PZ_Sliding_Tile>();

            spawnedTile.TileSetup(index + 1, 3); // 3�� �׸��� �� �׸���
            _tileList.Add(spawnedTile);
        }
    }

    private IEnumerator ShuffleTiles()
    {
        int shuffleCount = 20;
        float shuffleDuration = 0.01f;

        while (shuffleCount >= 0)
        {
            int tileIndex = Random.Range(0, _slidingPuzzleSize);

            // ���� Ÿ���� �� �ڷ� ������ ������ ���� ���
            _tileList[tileIndex].transform.SetAsLastSibling();
            shuffleCount--;
            yield return new WaitForSeconds(shuffleDuration);
        }

        // ������ ���� �� �� Ÿ���� ���� ��ġ�� ���� (3��)
        EmptyTilePosition = _tileList[2].GetComponent<RectTransform>().localPosition;
    }

    public void MoveTile(PZ_Sliding_Tile tile)
    {
        // Ŭ���� Ÿ���� �� Ÿ�ϰ� �پ� ���� ������ �� ������ �Ÿ��� _needMoveDistance�� ��ġ���� ����
        if (Vector3.Distance(EmptyTilePosition, tile.GetComponent<RectTransform>().localPosition) != _needMoveDistance)
        {
            return;
        }

        // �� Ÿ�� ��ġ�� �ٲٷ��� Ÿ�� ��ġ ���� ��ȯ
        Vector3 endPosition = EmptyTilePosition;
        EmptyTilePosition = tile.GetComponent<RectTransform>().localPosition;
        tile.TileMoveto(endPosition);
    }

    #endregion

    #region Clear

    public void CheckPuzzleClear()
    {
        if (_tileList.FindAll(tile => tile._isCorrect == true).Count == _slidingPuzzleSize - 1)
        {
            Debug.LogWarning("Sliding Puzzle Clear!!!");

            // ���⿡ ���� Ŭ���� ���� ���� ����
        }
    }

    #endregion
}