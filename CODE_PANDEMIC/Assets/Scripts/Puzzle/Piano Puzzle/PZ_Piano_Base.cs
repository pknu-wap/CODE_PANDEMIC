using UnityEngine;
using System.Collections.Generic;

public class PZ_Piano_Base : UI_PopUp
{
    private List<PZ_Piano_Tile_White> _whiteList = new List<PZ_Piano_Tile_White>();
    private List<PZ_Piano_Tile_Black> _blackList = new List<PZ_Piano_Tile_Black>();

    private string _correctPianoNotes = "SolSolLaLaSolSolMi"; // 정답
    private string _currentPianoNotes = ""; // 현재 선택된 음들

    private int _maxPianoCount = 7; // 최대 선택될 음 개수 ( == 정답 음의 개수)
    private int _currentPianoCount = 0; // 현재 선택된 음 개수

    private void Start()
    {
        GetSpawnedPianoTiles();
    }

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