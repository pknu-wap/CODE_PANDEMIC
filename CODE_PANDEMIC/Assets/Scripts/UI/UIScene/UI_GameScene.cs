using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    public UI_PlayerStatusBar StatusBar { get; private set; }
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
        StatusBar = GetObject((int)GameObjects.UI_PlayerStatusBar).GetComponent<UI_PlayerStatusBar>();
        Debug.Log("StatusBar");
        return true;
    }
    
}
