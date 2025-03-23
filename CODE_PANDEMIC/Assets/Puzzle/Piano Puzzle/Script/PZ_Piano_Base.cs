using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZ_Piano_Base : MonoBehaviour
{
    [SerializeField]
    private GameObject _tileWhitePrefab;

    [SerializeField]
    private GameObject _tileBlackPrefab;

    private List<PZ_Piano_Tile_White> _whiteList;
    private List<PZ_Piano_Tile_Black> _blackList;

    [SerializeField]
    private string _correctPianoNotes = "SolSolLaLaSolSolMi"; // 정답
    private string _currentPianoNotes = ""; // 현재 선택된 음들

    [SerializeField]
    private int _maxPianoCount = 7; // 최대 선택될 음 개수 ( == 정답 음의 개수)
    private int _currentPianoCount = 0; // 현재 선택된 음 개수

    private void Awake()
    {
        _whiteList = new List<PZ_Piano_Tile_White>();
        _blackList = new List<PZ_Piano_Tile_Black>();
    }

    private void Start()
    {
        SpawnPianoTiles();
    }

    // 피아노 타일 스폰
    private void SpawnPianoTiles()
    {
        // 흰 건반 스폰
        for (int i = 0; i < 7; i++)
        {
            GameObject spawnedTileObject = Instantiate(_tileWhitePrefab, transform);
            PZ_Piano_Tile_White spawnedTile = spawnedTileObject.GetComponent<PZ_Piano_Tile_White>();
            spawnedTile.TileSetup(i);
            _whiteList.Add(spawnedTile);
        }

        // 검은 건반 스폰
        for (int i = 0; i < 6; i++)
        {
            GameObject spawnedTileObject = Instantiate(_tileBlackPrefab, transform);
            PZ_Piano_Tile_Black spawnedTile = spawnedTileObject.GetComponent<PZ_Piano_Tile_Black>();
            spawnedTile.TileSetup(i);
            _blackList.Add(spawnedTile);
        }
    }

    // 퍼즐 클리어 여부 체크
    public void IsPuzzleClear(string selectedNote)
    {
        _currentPianoNotes += selectedNote;

        if (_currentPianoNotes == _correctPianoNotes)
        {
            Debug.LogWarning("Piano Puzzle Clear!!!");
            // 여기에 퍼즐 클리어 로직 구현
        }

        _currentPianoCount++;

        Debug.Log("누적된 피아노 음 : " + _currentPianoNotes);

        // 초기화
        if (_currentPianoCount >= _maxPianoCount)
        {
            _currentPianoNotes = "";
            Debug.LogWarning("Puzzle Reset");
        }
    }
}