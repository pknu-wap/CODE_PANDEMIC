using UnityEngine;
using System.Collections.Generic;

public class PZ_Piano_Base : PZ_Puzzle_Main
{
    #region Base

    private List<PZ_Piano_Tile_White> _whiteList = new List<PZ_Piano_Tile_White>();
    private List<PZ_Piano_Tile_Black> _blackList = new List<PZ_Piano_Tile_Black>();

    private List<string> _correctPianoNotes = new List<string>(); // 정답

    private int _maxPianoCount = 7; // 최대 선택될 음 개수 ( == 정답 음의 개수)
    private int _currentIndex = 0;

    private void Start()
    {
        SettingCorrectPianoNotes();
        GetSpawnedPianoTiles();
    }

    private void SettingCorrectPianoNotes()
    {
        _correctPianoNotes.Add("Sol");
        _correctPianoNotes.Add("Sol");
        _correctPianoNotes.Add("La");
        _correctPianoNotes.Add("La");
        _correctPianoNotes.Add("Sol");
        _correctPianoNotes.Add("Sol");
        _correctPianoNotes.Add("Mi");
    }

    #endregion

    #region Setting

    // 피아노 타일 가져오기
    private void GetSpawnedPianoTiles()
    {
        // 흰 건반 가져오기
        for (int index = 0; index < 7; index++)
        {
            Transform childTile = transform.GetChild(index);
            PZ_Piano_Tile_White spawnedTile = childTile.gameObject.GetComponent<PZ_Piano_Tile_White>();

            spawnedTile.TileSetup(index);
            _whiteList.Add(spawnedTile);
        }

        // 검은 건반 가져오기
        for (int index = 0; index < 6; index++)
        {
            Transform childTile = transform.GetChild(index + 7);
            PZ_Piano_Tile_Black spawnedTile = childTile.gameObject.GetComponent<PZ_Piano_Tile_Black>();

            spawnedTile.TileSetup(index);
            _blackList.Add(spawnedTile);
        }
    }

    #endregion

    #region Clear

    // 퍼즐 클리어 여부 체크
    public void CheckPuzzleClear(string selectedNote)
    {
        if (_correctPianoNotes[_currentIndex] != selectedNote)
        {
            Debug.LogWarning("Puzzle Reset");

            _currentIndex = 0;

            return;
        }

        if (_currentIndex == _maxPianoCount - 1 && _correctPianoNotes[_currentIndex] == selectedNote)
        {
            Debug.LogWarning("Piano Puzzle Clear!!!");

            PuzzleClear();

            _currentIndex = 0;

            return;
        }

        _currentIndex++;
    }

    protected override void PuzzleClear()
    {
        if (!_puzzleOwner)
        {
            return;
        }

        _puzzleOwner.ClearPuzzle();
    }

    #endregion
}