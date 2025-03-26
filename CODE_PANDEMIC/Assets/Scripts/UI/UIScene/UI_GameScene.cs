using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    // Start is called before the first frame update
    //미니맵
    //플레이어 status bar 
    //인벤토리 
    //몬스터 hp는 몬스터에게 붙일거고 
    //가이드 퀘스트 ?

    public override bool Init()
    {
        if (base.Init() == false) return false;


        return true;
    }
}
