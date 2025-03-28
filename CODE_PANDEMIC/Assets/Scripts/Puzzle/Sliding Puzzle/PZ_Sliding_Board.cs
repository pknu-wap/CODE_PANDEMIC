using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PZ_Sliding_Board : MonoBehaviour
{
    #region Base

    private RectTransform _rectTransform; // ũ�� ������ ���� ������
    private List<PZ_Sliding_Tile> _tileList = new List<PZ_Sliding_Tile>(); // ������ Ÿ���� ����
    private Vector2Int _slidingPuzzleSize = new Vector2Int(3, 3); // ���� ������
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

        SpawnTiles();

        // ��ġ ���� ���� ����ȭ
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());

        yield return new WaitForEndOfFrame();

        // ���� ��ġ�� ���� ��ġ�� ����
        _tileList.ForEach(tile => tile.SetCorrectPosition());

        StartCoroutine("ShuffleTiles");
    }

    #endregion

    #region Tile

    private void SpawnTiles()
    {
        for (int x = 0; x < _slidingPuzzleSize.x; x++)
        {
            for (int y = 0; y < _slidingPuzzleSize.y; y++)
            {
                PZ_Sliding_Tile spawnedTile = null;

                // Grid Layout Group�� Ȱ���� ���忡 Ÿ���� �����ϸ� �ڵ����� ��ġ�ǰ� ��
                Managers.Resource.Instantiate("PZ_Sliding_Tile", transform, (spawnedTileObject) =>
                {
                    spawnedTile = Utils.GetOrAddComponent<PZ_Sliding_Tile>(spawnedTileObject);
                });

                if (!spawnedTile.Init())
                {
                    break;
                }

                _tileList.Add(spawnedTile);
                spawnedTile.TileSetup(x * _slidingPuzzleSize.x + (y + 1), 3); // 3�� �׸��� �� �׸���
            }
        }
    }

    private IEnumerator ShuffleTiles()
    {
        int shuffleCount = 20;
        float shuffleDuration = 0.01f;

        while (shuffleCount >= 0)
        {
            int tileIndex = Random.Range(0, _slidingPuzzleSize.x * _slidingPuzzleSize.y);

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
        // ���� ����
        Debug.Log("Correct : " + _tileList.FindAll(tile => tile._isCorrect == true).Count);
        if (_tileList.FindAll(tile => tile._isCorrect == true).Count == _slidingPuzzleSize.x * _slidingPuzzleSize.y - 1)
        {
            // ���� ����
            Debug.LogWarning("Sliding Puzzle Clear!!!");

            // ���⿡ ���� Ŭ���� ���� ���� ����
        }
    }

    #endregion
}