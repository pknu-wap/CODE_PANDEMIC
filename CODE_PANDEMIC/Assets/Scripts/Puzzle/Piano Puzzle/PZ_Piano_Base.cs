using System.Collections.Generic;
using UnityEngine;

public class PZ_Piano_Base : MonoBehaviour
{
    private List<PZ_Piano_Tile_White> _whiteList = new List<PZ_Piano_Tile_White>();
    private List<PZ_Piano_Tile_Black> _blackList = new List<PZ_Piano_Tile_Black>();

    private string _correctPianoNotes = "SolSolLaLaSolSolMi"; // 정답
    private string _currentPianoNotes = ""; // 현재 선택된 음들

    private int _maxPianoCount = 7; // 최대 선택될 음 개수 ( == 정답 음의 개수)
    private int _currentPianoCount = 0; // 현재 선택된 음 개수

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

        // 검은 건반 스폰
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

    // 퍼즐 클리어 여부 체크
    public void CheckPuzzleClear(string selectedNote)
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