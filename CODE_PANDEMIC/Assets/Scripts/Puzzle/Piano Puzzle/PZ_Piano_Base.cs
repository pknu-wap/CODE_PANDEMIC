using UnityEngine;
using System.Collections.Generic;

public class PZ_Piano_Base : PZ_Puzzle_Main
{
    #region Base

    private List<PZ_Piano_Tile_White> _whiteList = new List<PZ_Piano_Tile_White>();
    private List<PZ_Piano_Tile_Black> _blackList = new List<PZ_Piano_Tile_Black>();

    private List<string> _correctPianoNotes = new List<string>(); // ����

    private int _maxPianoCount = 7; // �ִ� ���õ� �� ���� ( == ���� ���� ����)
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

    // �ǾƳ� Ÿ�� ��������
    private void GetSpawnedPianoTiles()
    {
        // �� �ǹ� ��������
        for (int index = 0; index < 7; index++)
        {
            Transform childTile = transform.GetChild(index);
            PZ_Piano_Tile_White spawnedTile = childTile.gameObject.GetComponent<PZ_Piano_Tile_White>();

            spawnedTile.TileSetup(index);
            _whiteList.Add(spawnedTile);
        }

        // ���� �ǹ� ��������
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

    // ���� Ŭ���� ���� üũ
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