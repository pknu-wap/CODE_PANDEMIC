using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static Define;

[Serializable]
public class GameData
{
    public int Chapter;
    public int Stage;

    public int ShortWeaponID;
    public int PistolID;
    public int RangeWeaponID;

    public int MasterVolume;
    public int BgmVolume;
    public int EffectVolume;
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
}
