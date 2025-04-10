using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class BaseScene : MonoBehaviour
{
    public SceneType SceneType = SceneType.Unknown;
    protected bool _init;
    private void Awake()
    {
        Init();
    }
    protected virtual bool Init()
    {
        if (_init) return false;
        _init = true;

        Managers.Init();
        GameObject obj = GameObject.Find("EventSystem");
        if (obj == null)
        {
           Managers.Resource.Instantiate("EventSystem", null);

       }
        return true;
    }
    public void FadeIn() //scene �Ѿ�� fade in fade out
    {
        //ui  manager ?

    } 

    public void FadeOut()
    {

    }

    public void Clear()
    {
        Debug.Log("clear");
    }
}
