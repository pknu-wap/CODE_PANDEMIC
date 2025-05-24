using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : SpawnBase
{
    SpawnerData _spawnerData;
    
    private void OnEnable()
    {
        Managers.Event.Subscribe("SpawnBossMonster", OnSpawnBossMonster);
        Managers.Event.Subscribe("OnBossClear", OnBossClear);
    }
    private void OnDisable()
    {
        Managers.Event.Unsubscribe("SpawnBossMonster", OnSpawnBossMonster);
        Managers.Event.Unsubscribe("OnBossClear", OnBossClear);
    }
    private void OnBossClear(object obj)
    {
        Destroy(gameObject);    
    }

    public void SetInfo(int id)
    {
        _spawnerData = Managers.Data.Spawners[id];
        Debug.Log(id);
        Register();
    }

    private void OnSpawnBossMonster(object obj)
    {
        SpawnObjects();
    }

    public override void SpawnObjects()
    {
        List<MonsterPosData> list = _spawnerData.Monsters; 
        for (int i = 0; i < list.Count; i++)
        {
            MonsterPosData monster = list[i];
            Managers.Data.Monsters.TryGetValue(monster.ID, out MonsterData data);
            if (data != null)
            {
                Managers.Resource.Instantiate(data.Prefab, transform, (obj) =>
                {
                    obj.transform.localPosition = monster.Pos;
                    AI_Base ai = obj.GetComponent<AI_Base>();
                    if (ai != null) ai.SetInfo(data);
                });
            }
        }
    }
}
