using System.Collections.Generic;
using UnityEngine;

public class PZ_Piano_Base : MonoBehaviour
{
    [SerializeField]
    private GameObject _tileWhitePrefab;

    [SerializeField]
    private GameObject _tileBlackPrefab;

    private List<PZ_Piano_Tile_White> _whiteList = new List<PZ_Piano_Tile_White>();
    private List<PZ_Piano_Tile_Black> _blackList = new List<PZ_Piano_Tile_Black>();

    [SerializeField]
    private string _correctPianoNotes = "SolSolLaLaSolSolMi"; // ����
    private string _currentPianoNotes = ""; // ���� ���õ� ����

    [SerializeField]
    private int _maxPianoCount = 7; // �ִ� ���õ� �� ���� ( == ���� ���� ����)
    private int _currentPianoCount = 0; // ���� ���õ� �� ����

    private bool Init()
    {
        if (!_tileWhitePrefab || !_tileBlackPrefab)
        {
            return false;
        }

        return true;
    }

    private void Start()
    {
        if (!Init())
        {
            return;
        }

        SpawnPianoTiles();
    }

    // �ǾƳ� Ÿ�� ����
    private void SpawnPianoTiles()
    {
        // �� �ǹ� ����
        for (int i = 0; i < 7; i++)
        {
            GameObject spawnedTileObject = Instantiate(_tileWhitePrefab, transform);
            PZ_Piano_Tile_White spawnedTile = spawnedTileObject.GetComponent<PZ_Piano_Tile_White>();

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
            GameObject spawnedTileObject = Instantiate(_tileBlackPrefab, transform);
            PZ_Piano_Tile_Black spawnedTile = spawnedTileObject.GetComponent<PZ_Piano_Tile_Black>();

            if (!spawnedTile.Init())
            {
                break;
            }

            spawnedTile.TileSetup(i);
            _blackList.Add(spawnedTile);
        }
    }

    // ���� Ŭ���� ���� üũ
    public void IsPuzzleClear(string selectedNote)
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