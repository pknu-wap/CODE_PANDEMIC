using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    enum GameObjects
    {
        UI_MiniMap,
        UI_PlayerStatusBar,
        UI_InGameSlot
    }  
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindObject(typeof(GameObjects));
        return true;
    }
}
