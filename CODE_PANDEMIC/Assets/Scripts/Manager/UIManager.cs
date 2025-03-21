using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    int _index = 20;
    Stack<UI_PopUp> _popupStack = new Stack<UI_PopUp>();
    
    public UI_Scene SceneUI { get; set; }

    //TODO : INVENTORY: NOT POPUP, NOT SCENE INVENTORY

    public  GameObject UIRoot
    {
        get
        {
            GameObject root = GameObject.Find("UI_Root");
            if (root == null) root = new GameObject { name = "UI_Root" };
            return root;
        }
    }

    //popup¿∫ sort scene not sort
    public void  SetCanvas(GameObject obj, bool sort =true)
    {
        Canvas canvas = Utils.GetOrAddComponent<Canvas>(obj);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        if (sort)
        {
            canvas.sortingOrder = _index;
            _index++;
        }
        else 
        {
            canvas.sortingOrder = 0;
        }
        
    }
    public void ShowSceneUI<T>(string key=null,Action<T>callback=null)where T : UI_Scene    
    {
        if(string.IsNullOrEmpty(key))
            key=typeof(T).Name;

        Managers.Resource.Instantiate(key, UIRoot.transform, (obj) =>
        {
            T sceneUI  = Utils.GetOrAddComponent<T>(obj);
            SceneUI = sceneUI;
            callback?.Invoke(sceneUI);
        }
        );
    }

    public void ShowPopupUI<T>(string key=null,Transform parent =null,Action<T>callback=null)where T :UI_PopUp
    {
        if (string.IsNullOrEmpty(key))
            key = typeof(T).Name;

        Managers.Resource.Instantiate(key, null, (obj) =>
        {
            T popup = Utils.GetOrAddComponent<T>(obj);
            _popupStack.Push(popup);
            if (parent != null) obj.transform.SetParent(parent);
            else obj.transform.SetParent(UIRoot.transform);
            callback?.Invoke(popup);
        });
    }
    public void ClosePopupUI(UI_PopUp popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_PopUp popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _index--;
    }
    public void CloseAllPopUI()
    {
        while (_popupStack.Count > 0) ClosePopupUI();
        SceneUI = null;
    }
}
