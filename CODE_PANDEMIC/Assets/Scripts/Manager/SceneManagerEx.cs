using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class SceneManagerEx : MonoBehaviour
{
    private SceneType _currentSceneType = SceneType.Unknown;
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public SceneType CurrentSceneType
    {
        get
        {
            if (CurrentSceneType != SceneType.Unknown) return _currentSceneType;
            else return CurrentScene.SceneType;
        }
        set { _currentSceneType = value;}
    }

    public void ChangScene(SceneType type)
    {
        CurrentScene.Clear();
        _currentSceneType = type;
        SceneManager.LoadScene(GetSceneName(type));
    }
    string GetSceneName(SceneType type)
    {
        string name =System.Enum.GetName(typeof(SceneType), type);
        char[] letters = name.ToLower().ToCharArray(); //������ �ҹ���
        letters[0] = char.ToUpper(letters[0]); //ù�ڴ� �빮�� 
        return new string(letters);
    }


}
