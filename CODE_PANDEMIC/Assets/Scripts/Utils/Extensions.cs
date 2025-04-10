using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Extensions
{
    public static void BindEvent(this GameObject obj, Action action, Define.UIEvent type = Define.UIEvent.Click)
    {
      //  UI_Base.BindEvent(obj, action, type);
    }
}