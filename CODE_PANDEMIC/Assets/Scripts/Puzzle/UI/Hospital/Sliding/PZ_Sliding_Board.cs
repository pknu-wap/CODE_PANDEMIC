using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PZ_Sliding_Board : PZ_Puzzle_UI_Main
{
    #region Base

    private List<PZ_Sliding_Tile> _tileList = new List<PZ_Sliding_Tile>();

    private int _slidingPuzzleSize = 9;
    private float _needMoveDistance = 235f;

    public Vector3 EmptyTilePosition { get; set; }

    private IEnumerator Start()
    {
        GetSpawnedTiles();

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());

        yield return new WaitForEndOfFrame();

        _tileList.ForEach(tile => tile.SetCorrectPosition());

        StartCoroutine("ShuffleTiles");
    }

    #endregion

    #region Setting

    private void GetSpawnedTiles()
    {
        GetComponentsInChildren(false, _tileList);

        for (int index = 0; index < _slidingPuzzleSize; index++)
        {
            _tileList[index].TileSetup(index + 1, 3);
        }
    }

    #endregion

    #region Tile

    private IEnumerator ShuffleTiles()
    {
        int shuffleCount = 20;
        float shuffleDuration = 0.01f;

        while (shuffleCount >= 0)
        {
            int tileIndex = Random.Range(0, _slidingPuzzleSize);

            _tileList[tileIndex].transform.SetAsLastSibling();
            shuffleCount--;
            yield return CoroutineHelper.WaitForSeconds(shuffleDuration);
        }

        EmptyTilePosition = _tileList[2].GetComponent<RectTransform>().localPosition;

        ReadyToPause();
    }

    public void MoveTile(PZ_Sliding_Tile tile)
    {
        if (Vector3.Distance(EmptyTilePosition, tile.GetComponent<RectTransform>().localPosition) != _needMoveDistance)
        {
            return;
        }

        Vector3 endPosition = EmptyTilePosition;
        EmptyTilePosition = tile.GetComponent<RectTransform>().localPosition;
        tile.TileMoveto(endPosition);
    }

    #endregion

    #region Clear

    public override void CheckPuzzleClear()
    {
        if (_tileList.FindAll(tile => tile._isCorrect == true).Count == _slidingPuzzleSize - 1)
        {
            PuzzleClear();
        }
    }

    #endregion
}