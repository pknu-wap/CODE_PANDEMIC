using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    UI_GameScene _gameSceneUI;
    StageData _stageData;
    protected override bool Init()
    {
        if (base.Init() == false) return false;
        SceneType = Define.SceneType.GameScene;

        StartCoroutine(CowaitLoad());
        return true;
    }
    IEnumerator CowaitLoad()
    {
        while (Managers.Data.Loaded() == false) yield return null;
    }
}
