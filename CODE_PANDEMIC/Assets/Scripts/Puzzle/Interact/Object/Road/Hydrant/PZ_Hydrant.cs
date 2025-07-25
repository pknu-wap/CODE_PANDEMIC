using UnityEngine;

public class PZ_Hydrant : PZ_Interact_NonSpawn
{
    [SerializeField] private GameObject _water;
    [SerializeField] private int _spawnCount = 2;

    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        SpawnWaters();
    }

    private void SpawnWaters()
    {
        for (int i = 1; i <= _spawnCount; i++)
        {
            SpawnWater(-i, 0);
            SpawnWater(i, 0);
            SpawnWater(0, -i);
            SpawnWater(0, i);
        }
    }

    private void SpawnWater(int plusX, int plusY)
    {
        Vector3 spawnPos = new Vector3(transform.position.x + plusX, transform.position.y + plusY, 0);

        Instantiate(_water, spawnPos, Quaternion.identity);
    }
}