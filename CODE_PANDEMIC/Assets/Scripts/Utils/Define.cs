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
        GameScene,
        TitleScene,
    };
    public enum WeaponType
    {
        None,
        ShortWeapon,
        PistolWeapon,
        RangeWeapon
    }
    public enum ItemType
    {
        Edible,
        Equippable
    }
    public const int STAGES_PER_CHAPTER = 3;
}
