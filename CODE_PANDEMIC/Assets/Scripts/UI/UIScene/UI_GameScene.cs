using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    // Start is called before the first frame update
    //�̴ϸ�
    //�÷��̾� status bar 
    //�κ��丮 
    //���� hp�� ���Ϳ��� ���ϰŰ� 
    //���̵� ����Ʈ ?

    public override bool Init()
    {
        if (base.Init() == false) return false;


        return true;
    }
}
