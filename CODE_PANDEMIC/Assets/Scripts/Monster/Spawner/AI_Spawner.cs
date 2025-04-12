using UnityEngine;

public class AI_Spawner : SpawnBase
{
    [SerializeField] private GameObject[] _enemyPrefabs; 

    protected override void SpawnObj()
    {

        foreach (MonsterPosData monsterData in spawnerData.Monsterss)
        {

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
