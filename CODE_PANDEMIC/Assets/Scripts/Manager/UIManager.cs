using Inventory.UI;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    int _index = 20;
    Stack<UI_PopUp> _popupStack = new Stack<UI_PopUp>();
    GameObject _fadeImage;
    public UI_Inventory InventoryUI { get; private set; }
    public UI_Scene SceneUI { get; set; }
    public UI_EnlargedMiniMap EnlargedMiniMapUI { get; private set; }

    private GameObject _uiRoot;

    public GameObject UIRoot
    {
        get
        {
            if (_uiRoot == null)
            {
                _uiRoot = GameObject.Find("UI_Root");
                if (_uiRoot == null)
                {
                    _uiRoot = new GameObject { name = "UI_Root" };
                    
                }
            }
            return _uiRoot;
        }
    }

    public void SetCanvas(GameObject obj, bool sort = true)
    {
        Canvas canvas = Utils.GetOrAddComponent<Canvas>(obj);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = sort ? _index++ : 0;
    }

    public void ShowSceneUI<T>(string key = null, Action<T> callback = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(key))
            key = typeof(T).Name;

        Managers.Resource.Instantiate(key, UIRoot.transform, (obj) =>
        {
            T sceneUI = Utils.GetOrAddComponent<T>(obj);
            SceneUI = sceneUI;
            callback?.Invoke(sceneUI);
        });
    }
    
    public void ShowPopupUI<T>(string key = null, Transform parent = null, Action<T> callback = null) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(key))
            key = typeof(T).Name;

        Managers.Resource.Instantiate(key, null, (obj) =>
        {
            T popup = Utils.GetOrAddComponent<T>(obj);
            _popupStack.Push(popup);
            obj.transform.SetParent(parent != null ? parent : UIRoot.transform, false);
            callback?.Invoke(popup);
            
        });

    }

    public void ClosePopupUI(UI_PopUp popup)
    {
        if (_popupStack.Count == 0 || _popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }
        ClosePopupUI();
    }
    public void CloseEnlargedMiniMap()
    {
        if (EnlargedMiniMapUI != null)
        {
            Destroy(EnlargedMiniMapUI.gameObject);
            EnlargedMiniMapUI = null;
        }
    }
    public void OpenEnlargedMiniMap()
    {
        Managers.Resource.Instantiate("UI_EnlargedMiniMap", UIRoot.transform, obj =>
        {
            EnlargedMiniMapUI = obj.GetComponent<UI_EnlargedMiniMap>();
            SetCanvas(obj, true);
        });
    }         

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_PopUp popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        _index--;
    }

    public void CloseAllPopUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
        SceneUI = null;
    }

    public bool HasPopUpUI()
    {
        return _popupStack.Count > 0;
    }
    
    public void ShowInventoryUI(Action<UI_Inventory> callback = null)
    {
        if (InventoryUI == null)
        {
            Transform parent = SceneUI != null ? SceneUI.transform : UIRoot.transform;

            Managers.Resource.Instantiate("UI_Inventory", parent, (obj) =>
            {
                InventoryUI = Utils.GetOrAddComponent<UI_Inventory>(obj);
                InventoryUI.Init(); 
                SetCanvas(obj, false);
                callback?.Invoke(InventoryUI);
              
                
            });
        }
        else
        {
            InventoryUI.Show();
            callback?.Invoke(InventoryUI);
        }
    }
    public bool IsOpenInventory()
    {
        if(!InventoryUI) return false;  
        if(InventoryUI.gameObject.activeSelf==false) return false;
        return true;
        
    }
    public void HideInventoryUI()
    {
        if (InventoryUI != null)
            InventoryUI.Hide();
    }

    public void CloseInventoryUI()
    {
        if (InventoryUI != null)
        {
            Managers.Resource.Destroy(InventoryUI.gameObject);
            InventoryUI = null;
        }
    }

    public bool IsInventoryVisible()
    {
        return InventoryUI != null && InventoryUI.gameObject.activeSelf;
    }

    public void FadeAtOnce()
    {
        if (_fadeImage == null)
        {
            Managers.Resource.Instantiate("UI_FadeImage", UIRoot.transform, (obj) =>
            {
                _fadeImage = obj;
                SetCanvas(obj);
                var fade = _fadeImage.GetOrAddComponent<UI_FadeImage>();
                fade.Init();
                fade.FadeAtOnce();
            });
        }
        else
        {
            _fadeImage.GetOrAddComponent<UI_FadeImage>().FadeAtOnce();
        }
    }

    public void FadeIn(Action onComplete = null)
    {
        if (_fadeImage == null)
        {
            Managers.Resource.Instantiate("UI_FadeImage", UIRoot.transform, (obj) =>
            {
                _fadeImage = obj;
                SetCanvas(obj);
                var fade = _fadeImage.GetOrAddComponent<UI_FadeImage>();
                fade.Init();
                fade.FadeIn(onComplete);
            });
        }
        else
        {
            _fadeImage.SetActive(true);
            _fadeImage.GetComponent<UI_FadeImage>().FadeIn(onComplete);
        }
    }

    public void FadeOut(Action onComplete = null)
    {
        if (_fadeImage == null)
        {
            Managers.Resource.Instantiate("UI_FadeImage", UIRoot.transform, (obj) =>
            {
                _fadeImage = obj;
                SetCanvas(obj);
                var fade = _fadeImage.GetOrAddComponent<UI_FadeImage>();
                fade.Init();
                fade.FadeOut(onComplete);
            });
        }
        else
        {
            _fadeImage.SetActive(true);
            _fadeImage.GetOrAddComponent<UI_FadeImage>().FadeOut(onComplete);
        }
    }

    public UI_EquipPopUp EquipPopUpUI { get; private set; }

    public void ShowEquipPopUpUI(Action<UI_EquipPopUp> callback = null)
    {
        if (EquipPopUpUI == null)
        {
            Transform parent = SceneUI != null ? SceneUI.transform : UIRoot.transform;

            Managers.Resource.Instantiate("UI_EquipPopUp", parent, (obj) =>
            {
                EquipPopUpUI = Utils.GetOrAddComponent<UI_EquipPopUp>(obj);
                SetCanvas(obj, false);
                callback?.Invoke(EquipPopUpUI);
            });
        }
        else
        {
            EquipPopUpUI.gameObject.SetActive(true);
            callback?.Invoke(EquipPopUpUI);
        }
    }
    public bool HasEquipPopUpUI()
    {
        return EquipPopUpUI!=null;
    }
    public void CloseEquipPopUpUI()
    {
        if (EquipPopUpUI != null)
        {
            Managers.Resource.Destroy(EquipPopUpUI.gameObject);
            EquipPopUpUI = null;
        }
    }

   
    public void ClearUI()
    {
        CloseAllPopUI();

        if (InventoryUI != null)
        {
            Destroy(InventoryUI.gameObject);
            Debug.Log("Destroy");
            InventoryUI = null;
        }

        if (SceneUI != null)
        {
           Destroy(SceneUI.gameObject);
            SceneUI = null;
        }

        if(EnlargedMiniMapUI != null)
        {
            CloseEnlargedMiniMap();
        }

        if (HasEquipPopUpUI())
        {
            CloseEquipPopUpUI();
        }

        CloseAllPopUI();
    }
}
