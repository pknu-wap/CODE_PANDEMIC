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
public static class CoroutineHelper
{
    private static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    private static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private static Dictionary<float, WaitForSeconds> _WaitForSeconds;
    private static Dictionary<float, WaitForSecondsRealtime> _WaitForSecondsRealtime;

    static CoroutineHelper() // 정적 생성자
    {
        _WaitForSeconds = new Dictionary<float, WaitForSeconds>();
        _WaitForSecondsRealtime = new Dictionary<float, WaitForSecondsRealtime>();
    }

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!_WaitForSeconds.TryGetValue(seconds, out var waitForSeconds)) 
        {
            _WaitForSeconds.Add(seconds, waitForSeconds = new WaitForSeconds(seconds));
        }
        return waitForSeconds;
    }

    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
    {
        if (!_WaitForSecondsRealtime.TryGetValue(seconds, out var waitForSecondsRealtime))
        {
            _WaitForSecondsRealtime.Add(seconds, waitForSecondsRealtime = new WaitForSecondsRealtime(seconds));
        }
        return waitForSecondsRealtime;
    }
}