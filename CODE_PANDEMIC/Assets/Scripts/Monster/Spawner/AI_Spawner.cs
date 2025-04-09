using UnityEngine;

public class AI_Spawner : SpawnBase
{
    [SerializeField] private GameObject[] _enemyPrefabs; 

    protected override void SpawnObj()
    {

        foreach (MonsterPosData monsterData in spawnerData.Monsterss)
        {
            if (monsterData.ID < 0 || monsterData.ID >= _enemyPrefabs.Length)
            {
                Debug.LogWarning($"유효하지 않은 몬스터 ID: {monsterData.ID}");
                continue;
            }

            GameObject prefab = _enemyPrefabs[monsterData.ID];
            Vector2Int spawnPos = monsterData.Pos; 

            GameObject enemyObj = Instantiate(prefab, new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);

            AI_Controller aiController = enemyObj.GetComponent<AI_Controller>();
            if (aiController != null)
            {
                aiController.Init();
            }
        }
    }
}
