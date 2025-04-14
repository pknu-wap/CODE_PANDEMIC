using System.Collections.Generic;
using UnityEngine;

public class AI_Spawner : SpawnBase
{
  

    private SpawnerData _spawnerData;
    private List<GameObject> _monsters = new List<GameObject>();
    private int _spawnCount = 0;
    private bool _isDespawning = false;

    public void SetInfo(int id)
    {
        _spawnerData = Managers.Data.Spawners[id];
        Register();
    }

    public override void SpawnObjects()
    {
        Debug.Log($"id :{_spawnerData.TemplateID}");
    }

    public void UnRegisterMonster(GameObject monster)
    {
        _monsters.Remove(monster);
        _spawnCount--;

        if (_spawnCount <= 0)
        {
            UnRegister();
            Destroy(gameObject);
            Debug.Log("All monsters from this spawner are defeated.");
        }
    }

    public void DespawnAll()
    {
        if (_isDespawning) return;
        _isDespawning = true;

        foreach (var monster in _monsters)
            if (monster != null)
                Destroy(monster);

        _monsters.Clear();
        _spawnCount = 0;
    }

    protected override void OnDestroy()
    {
        DespawnAll();
        base.OnDestroy();
    }
}
