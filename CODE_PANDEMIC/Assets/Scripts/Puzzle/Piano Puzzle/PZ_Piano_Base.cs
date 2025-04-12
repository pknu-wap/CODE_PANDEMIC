using UnityEngine;
using System.Collections.Generic;

public class PZ_Piano_Base : PZ_Puzzle_Base
{
    #region Base

    private List<PZ_Piano_Tile_White> _whiteList = new List<PZ_Piano_Tile_White>();
    private List<PZ_Piano_Tile_Black> _blackList = new List<PZ_Piano_Tile_Black>();

    private string _correctPianoNotes = "SolSolLaLaSolSolMi"; // ����
    private string _currentPianoNotes = ""; // ���� ���õ� ����

    private int _maxPianoCount = 7; // �ִ� ���õ� �� ���� ( == ���� ���� ����)
    private int _currentPianoCount = 0; // ���� ���õ� �� ����

    private void Start()
    {
        GetSpawnedPianoTiles();
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
        _currentPianoNotes += selectedNote;

        if (_currentPianoNotes == _correctPianoNotes)
        {
            PuzzleClear();

            return;
        }

        _currentPianoCount++;

        Debug.Log("������ �ǾƳ� �� : " + _currentPianoNotes);

        // �ʱ�ȭ
        if (_currentPianoCount >= _maxPianoCount)
        {
            _currentPianoNotes = "";
            _currentPianoCount = 0;
            Debug.LogWarning("Puzzle Reset");
        }
    }

    protected override void PuzzleClear()
    {
        if (!_puzzleOwner)
        {
            return;
        }

        Debug.LogWarning("Piano Puzzle Clear!!!");

        _puzzleOwner.ClearPuzzle();
    }

    #endregion
}