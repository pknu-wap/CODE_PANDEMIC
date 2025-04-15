﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PZ_Sliding_Board : PZ_Puzzle_Main
{
    #region Base

    private List<PZ_Sliding_Tile> _tileList = new List<PZ_Sliding_Tile>(); // 생성된 타일을 저장

    private int _slidingPuzzleSize = 9; // 퍼즐 사이즈
    private float _needMoveDistance = 235f; // 타일이 이동해야 하는 거리

    public Vector3 EmptyTilePosition { get; set; } // 빈 타일의 위치

    private IEnumerator Start()
    {
        GetSpawnedTiles();

        // 위치 정보 강제 동기화
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());

        yield return new WaitForEndOfFrame();

        // 현재 위치를 정답 위치로 간주
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
            _tileList[index].TileSetup(index + 1, 3); // 3번 그림이 빈 그림임
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

            // 랜덤 타일을 맨 뒤로 보내서 순서를 섞는 방식
            _tileList[tileIndex].transform.SetAsLastSibling();
            shuffleCount--;
            yield return new WaitForSeconds(shuffleDuration);
        }

        // 셔플이 끝난 후 빈 타일의 현재 위치를 저장 (3번)
        EmptyTilePosition = _tileList[2].GetComponent<RectTransform>().localPosition;
    }

    public void MoveTile(PZ_Sliding_Tile tile)
    {
        // 클릭한 타일이 빈 타일과 붙어 있지 않으면 둘 사이의 거리가 _needMoveDistance와 일치하지 않음
        if (Vector3.Distance(EmptyTilePosition, tile.GetComponent<RectTransform>().localPosition) != _needMoveDistance)
        {
            return;
        }

        // 빈 타일 위치와 바꾸려는 타일 위치 정보 교환
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
            PuzzleClear();
        }
    }

    protected override void PuzzleClear()
    {
        if (!_puzzleOwner)
        {
            return;
        }

        Debug.LogWarning("Sliding Puzzle Clear!!!");

        _puzzleOwner.ClearPuzzle();
    }

    #endregion
}