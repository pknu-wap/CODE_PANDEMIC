using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PZ_Sliding_Board : MonoBehaviour
{
    [SerializeField]
    private GameObject _tilePrefab; // 타일 프리팹
    private RectTransform _rectTransform; // 크기 조정을 위해 가져옴
    private List<PZ_Sliding_Tile> _tileList; // 생성된 타일을 저장
    private Vector2Int _slidingPuzzleSize = new Vector2Int(3, 3); // 퍼즐 사이즈
    private float _needMoveDistance = 232f; // 타일이 이동해야 하는 거리

    public Vector3 EmptyTilePosition { get; set; } // 빈 타일의 위치

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        // 보드 기본 설정
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = Vector2.zero;
        _rectTransform.sizeDelta = new Vector2(730, 730);

        _tileList = new List<PZ_Sliding_Tile>();
    }

    private IEnumerator Start()
    {
        SpawnTiles();

        // 위치 정보 강제 동기화
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());

        yield return new WaitForEndOfFrame();

        // 현재 위치를 정답 위치로 간주
        _tileList.ForEach(tile => tile.SetCorrectPosition());

        StartCoroutine("ShuffleTiles");
    }

    private void SpawnTiles()
    {
        for (int x = 0; x < _slidingPuzzleSize.x; x++)
        {
            for (int y = 0; y < _slidingPuzzleSize.y; y++)
            {
                // Grid Layout Group을 활용해 보드에 타일을 스폰하면 자동으로 배치되게 함
                GameObject spawnedTileObject = Instantiate(_tilePrefab, transform);
                PZ_Sliding_Tile spawnedTile = spawnedTileObject.GetComponent<PZ_Sliding_Tile>();
                _tileList.Add(spawnedTile);
                spawnedTile.TileSetup(x * _slidingPuzzleSize.x + (y + 1), 3); // 3번 그림이 빈 그림임
            }
        }
    }
    private IEnumerator ShuffleTiles()
    {
        int shuffleCount = 20;
        float shuffleDuration = 0.01f;

        while (shuffleCount >= 0)
        {
            int tileIndex = Random.Range(0, _slidingPuzzleSize.x * _slidingPuzzleSize.y);

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

    public void IsPuzzleClear()
    {
        // 삭제 예정
        Debug.Log("Correct : " + _tileList.FindAll(tile => tile._isCorrect == true).Count);
        if (_tileList.FindAll(tile => tile._isCorrect == true).Count == _slidingPuzzleSize.x * _slidingPuzzleSize.y - 1)
        {
            // 삭제 예정
            Debug.Log("Puzzle Clear!!!");

            // 여기에 퍼즐 클리어 로직 구현 예정
        }
    }
}