using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class TitleScene : BaseScene
{
    UI_TitleScene _titleSceneUI;

    protected override bool Init()
    {
        if (base.Init() == false) return false;
        SceneType = Define.SceneType.TitleScene;

        Managers.UI.ShowSceneUI<UI_TitleScene>(callback: (titleSceneUI) =>
        {
            _titleSceneUI = titleSceneUI;

        });
        return true;
    }

}
