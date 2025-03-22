using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Managers : MonoBehaviour
{
    private static Managers _instance;
  
    ObjectManager objectManager;

    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    GameManagerEx _game =new GameManagerEx();
    SceneManagerEx _scene = new SceneManagerEx();
    public static Managers Instance { get { return _instance; } }
    public static ResourceManager Resource { get { return _instance._resource; }}
    public static UIManager UI { get { return _instance._ui; } }

    public static SceneManagerEx Scene { get { return _instance._scene; } }
    public static GameManagerEx Game { get { return _instance._game; } }

    public static bool Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("Managers");
            if (go == null)
                go = new GameObject { name = "Managers" };

            _instance = go.GetOrAddComponent<Managers>();
            DontDestroyOnLoad(go);
            

        }
        return true;
    }

   
}