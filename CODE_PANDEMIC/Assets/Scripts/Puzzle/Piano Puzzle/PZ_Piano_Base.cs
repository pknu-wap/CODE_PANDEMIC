using System.Collections.Generic;
using UnityEngine;

public class PZ_Piano_Base : MonoBehaviour
{
    private List<PZ_Piano_Tile_White> _whiteList = new List<PZ_Piano_Tile_White>();
    private List<PZ_Piano_Tile_Black> _blackList = new List<PZ_Piano_Tile_Black>();

    private string _correctPianoNotes = "SolSolLaLaSolSolMi"; // ����
    private string _currentPianoNotes = ""; // ���� ���õ� ����

    private int _maxPianoCount = 7; // �ִ� ���õ� �� ���� ( == ���� ���� ����)
    private int _currentPianoCount = 0; // ���� ���õ� �� ����

    private void Start()
    {
        SpawnPianoTiles();
    }

    // �ǾƳ� Ÿ�� ����
    private void SpawnPianoTiles()
    {
        // �� �ǹ� ����
        for (int i = 0; i < 7; i++)
        {
            PZ_Piano_Tile_White spawnedTile = null;

            Managers.Resource.Instantiate("PZ_Piano_Tile_White", transform, (spawnedTileObject) =>
            {
                spawnedTile = Utils.GetOrAddComponent<PZ_Piano_Tile_White>(spawnedTileObject);
            });

            if (!spawnedTile.Init())
            {
                break;
            }

            spawnedTile.TileSetup(i);
            _whiteList.Add(spawnedTile);
        }

        // ���� �ǹ� ����
        for (int i = 0; i < 6; i++)
        {
            PZ_Piano_Tile_Black spawnedTile = null;

            Managers.Resource.Instantiate("PZ_Piano_Tile_Black", transform, (spawnedTileObject) =>
            {
                spawnedTile = Utils.GetOrAddComponent<PZ_Piano_Tile_Black>(spawnedTileObject);
            });

            if (!spawnedTile.Init())
            {
                break;
            }

            spawnedTile.TileSetup(i);
            _blackList.Add(spawnedTile);
        }
    }

    // ���� Ŭ���� ���� üũ
    public void CheckPuzzleClear(string selectedNote)
    {
        _currentPianoNotes += selectedNote;

        if (_currentPianoNotes == _correctPianoNotes)
        {
            Debug.LogWarning("Piano Puzzle Clear!!!");
            // ���⿡ ���� Ŭ���� ���� ����
        }

        _currentPianoCount++;

        Debug.Log("������ �ǾƳ� �� : " + _currentPianoNotes);

        // �ʱ�ȭ
        if (_currentPianoCount >= _maxPianoCount)
        {
            _currentPianoNotes = "";
            Debug.LogWarning("Puzzle Reset");
        }
    }
}