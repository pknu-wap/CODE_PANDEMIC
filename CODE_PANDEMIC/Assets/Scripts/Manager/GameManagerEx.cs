using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static Define;

[Serializable]
public class GameData
{
    public int Chapter=1;
    public int Stage=1;

    public int ShortWeaponID;
    public int PistolID;
    public int RangeWeaponID;

    public int MasterVolume;
    public int BgmVolume;
    public int EffectVolume;

    public Resolution SaveResolution;
    public bool IsFullScreen;
}
public class GameManagerEx : MonoBehaviour
{
   
    GameData _gameData = new GameData();
    public GameData SaveData
    { 
        get { return _gameData; } 
        set { _gameData = value; } 
    }
    public void Init()
    {
        _path = Application.persistentDataPath + "/SaveData.json";
    }
    public void SetResolutionScreen(Resolution res)
    {
        Screen.SetResolution(res.width, res.height, Screen.fullScreen=SaveData.IsFullScreen);
        Debug.Log($"{res.width}x{res.height}");
        SaveData.SaveResolution = res;
    }
    public void SetFullScreenMode(bool value)
    {
        Screen.fullScreen = value;
        SaveData.IsFullScreen = value;
    }
    #region SaveLoad
    public bool IsLoaded = false;
    string _path;
    public void SaveGame()
    {
        string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
        File.WriteAllText(_path, jsonStr);
    }
    public bool LoadGame()
    {
        if (File.Exists(_path) == false)
            return false;

        string fileStr = File.ReadAllText(_path);
        GameData data = JsonUtility.FromJson<GameData>(fileStr);
        if (data != null)
            Managers.Game.SaveData = data;

        IsLoaded = true;
        return true;
    }
    #endregion

    public void QuitGame()
    {
        //����Ƽ  ������ ���ø����̼� �ΰ��� ��Ȳ �ٳ������ְ� 
        //���� ����Ƽ �����͸� ���� �� 
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
        #else
        Application.Quit();
        #endif
    
    }
}
