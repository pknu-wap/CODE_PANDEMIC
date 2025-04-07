using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;

    [SerializeField] private Vector2[] _spawnPositions = // 나중에 csv 혹은 json으로 대체 예정 
    {
        new Vector2(-4f,  3f) // 임시값
    };

    [SerializeField] private int _initialPoolSize = 10;

    private Queue<GameObject> _pool;
    private int _nextPrefabIndex = 0;
    private int _nextSpawnIndex  = 0;

    private void Awake()
    {
        InitializePool();
    }

    private void Start()
    {
        SpawnAllOnce();
    }

    private void InitializePool()
    {
        _pool = new Queue<GameObject>();
        for (int i = 0; i < _initialPoolSize; i++)
        {
            GameObject obj = Instantiate(_enemyPrefabs[0]);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    private GameObject GetFromPool()
    {
        GameObject obj;
        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
        }
        else
        {
            // 풀 비어있으면 다음 순서 프리팹 인스턴스화
            obj = Instantiate(_enemyPrefabs[_nextPrefabIndex]);
        }
        obj.SetActive(true);
        return obj;
    }

    private void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }

    private void SpawnAllOnce()
    {
        int spawnCount = _spawnPositions.Length;
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnNext();
        }
    }

    private void SpawnNext()
    {
        // 풀에서 오브젝트 가져오기
        GameObject enemyObj = GetFromPool();

        // 위치 설정
        Vector2 spawnPos = _spawnPositions[_nextSpawnIndex];
        enemyObj.transform.position = spawnPos;
        enemyObj.transform.rotation = Quaternion.identity;

        // 프리팹 교체(가져온 오브젝트가 다른 프리팹일 수 있으므로)
        GameObject desiredPrefab = _enemyPrefabs[_nextPrefabIndex];
        if (enemyObj.name.Contains(desiredPrefab.name) == false)
        {
            Destroy(enemyObj);
            enemyObj = Instantiate(desiredPrefab, spawnPos, Quaternion.identity);
        }

        // AI 초기화
        var aiController = enemyObj.GetComponent<AI_Controller>();
        if (aiController != null)
        {
            aiController.Init();
        }

        // 사망 시 풀로 반환
        var aiBase = enemyObj.GetComponent<AI_Base>();
        if (aiBase != null)
        {
            aiBase.OnDie += () => ReturnToPool(enemyObj);
        }

        // 인덱스 순환
        _nextPrefabIndex = (_nextPrefabIndex + 1) % _enemyPrefabs.Length;
        _nextSpawnIndex  = (_nextSpawnIndex  + 1) % _spawnPositions.Length;
    }
}
