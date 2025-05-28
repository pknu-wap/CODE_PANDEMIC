using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UI_DiaryPopUp : UI_PopUp
{
    enum Texts
    {
        ItemCount,
        MonsterCount,
        PuzzleCount,
        InteractCount,
    }
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindText(typeof(Texts));
        return true;
    }
        
    public void WriteDiary()
    {
        GetText((int)Texts.ItemCount).text = $"얻은 아이템의 수 : {Managers.Game.Record.ItemCount}";
        GetText((int)Texts.MonsterCount).text = $"처치한 좀비의 수 : {Managers.Game.Record.ZombieKillCount}";
        GetText((int)Texts.PuzzleCount).text = $"클리어한 퍼즐의 수 : {Managers.Game.Record.ClearPuzzleCount}";
        GetText((int)Texts.InteractCount).text = $"얻은 아이템의 수 : {Managers.Game.Record.Interacts.GetAllCount()}";
    }


}
