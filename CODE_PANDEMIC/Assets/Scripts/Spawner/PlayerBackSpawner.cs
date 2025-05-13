using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBackSpawner : SpawnBase
{
    public override void SpawnObjects()
    {
        Managers.Object.RegisterSpawners();
        if (Managers.Game.IsNextStage()) return;
        Managers.Resource.Instantiate("Player", null, (obj) =>
        {
            obj.transform.position = transform.position;
            obj.GetOrAddComponent<PlayerStatus>().SetInfo();
            Managers.Object.RegisterPlayer(obj.GetComponent<PlayerStatus>());

        });

    }
}
