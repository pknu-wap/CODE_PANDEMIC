using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        Weaponable,
        Interact,
        Equippable,
        Buff
    }
    public enum ActionType
    { 
        None,
        Equip,
        QuickSlot,
        Buff,
    }
   public enum ArmorType
    {
        None,
        Helmet,
        Armor,
        Shoes
    }
    public enum BuffType
    {
        None,
        Health,
        Defend,
        Speed,
        Attack
    }
    public enum CinematicType
    {
        PuzzleClear,
        BossSequence
    }
    public const int STAGES_PER_CHAPTER = 4; //3 normal 1 boss

    public const int ArmorCount = 3;

    public const int None = 0;
    public const int PlayerCamera = 10;
    public const int Cinematic = 20;


    public enum InteractType
    {
        Generator,
        Extinguisher,
        MainPuzzle,
        Safe,
        Container,
        Traffic
    }
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
public static class EquipSlotIndex
{
    public const int Helmet= 0;
    public const int Armor = 1;
    public const int Shoes = 2;

    public static int GetSlotIndex(Define.ArmorType type)
    {
        return type switch
        {
            Define.ArmorType.Helmet => Helmet,
            Define.ArmorType.Armor => Armor,
            Define.ArmorType.Shoes => Shoes,
            _ => -1
        };
    }
}