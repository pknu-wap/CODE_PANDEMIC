using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    UI_GameScene _gameSceneUI;
    protected override bool Init()
    {
        if (base.Init() == false) return false;
        SceneType = Define.SceneType.GameScene;
        return true;
    }
}
