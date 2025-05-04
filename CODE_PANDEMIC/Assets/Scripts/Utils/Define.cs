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
        Equippable,
        Interact
    }
    public enum ActionType
    { 
        None,
        Equip,
        QuickSlot,
        Buff,
    }
   
    public const int STAGES_PER_CHAPTER = 4; //3 normal 1 boss

    public const int None = 0;
    public const int PlayerCamera = 11;
    public const int PuzzleClear = 20;
    public const int BossSequence = 30;
}
public static class QuickSlotIndex
{
    public const int ShortWeapon = 1;
    public const int PistolWeapon = 2;
    public const int RangeWeapon = 3;
    public const int Portion = 4;

    public static int GetSlotIndex(Define.WeaponType type)
    {
        return type switch
        {
            Define.WeaponType.ShortWeapon => ShortWeapon,
            Define.WeaponType.PistolWeapon => PistolWeapon,
            Define.WeaponType.RangeWeapon => RangeWeapon,
            _ => Portion
        };
    }
}