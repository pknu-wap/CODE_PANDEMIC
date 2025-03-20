using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum UIEvent
    {
        Click,
        Press
    }
    public enum UIDragEvent
    {
        Drag,
        DragEnd,
    }
    public enum SceneType
    {
        Unknown,
        TitleScene,
        GameScene
    };
}
