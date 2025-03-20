using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

//건들지마세요 
public class Utils
{
    public static T GetOrAddComponent<T>(GameObject obj) where T : UnityEngine.Component
    {
        if (obj == null) return null;
        T component = obj.GetComponent<T>();
        if (component == null) component = obj.AddComponent<T>();
        return component;
    }

    public static T FindChild<T>(GameObject obj, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (obj == null) return null;
        if (recursive == false)
        {
            Transform transform = obj.transform.Find(name);
            if (transform != null) return transform.GetComponent<T>();
        }
        else
        {
            foreach (T component in obj.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
    public static GameObject FindChild(GameObject obj, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(obj, name, recursive);
        if (transform != null) return transform.gameObject;
        return null;
    }
}
