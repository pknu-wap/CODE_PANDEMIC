using Inventory.Model;
using Inventory.Model.Inventory.Model;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

#region GameData 구조체 정의
[Serializable]
public class QuickSlotItemData
{
    public int ItemID;
    public int Quantity;
}

[Serializable]
public class GameData
{
    public int HighestChapter = 1;
    public int HighestStage = 1;
    public int Chapter =1;
    public int Stage = 1;
    public QuickSlotItemData[] QuickSlotItems = new QuickSlotItemData[4];

    public int MasterVolume;
    public int BgmVolume;
    public int EffectVolume;

    public Resolution SaveResolution;
    public bool IsFullScreen;
    public static string FilePath => Application.persistentDataPath + "/SaveData.json";
}
#endregion

#region GameManagerEx
public class GameManagerEx
{
    private QuickSlot _quickSlot;
    private bool _isPaused;
    
    private GameData _gameData = new GameData();
    private GameSaver _gameSaver;
    
    private InventoryData _inventoryData;
    private InventorySaver _inventorySaver;
    
    private StageProgressData _stageProgressData;
    private StageProgressSaver _stageProgressSaver;

    private HashSet<int> _clearPuzzleID;
    private HashSet<int> _obtainedItemIDs;
    private HashSet<int> _interactObjects;

    private int _prevStage;
    private int _prevChapter;

    public HashSet<int> ObtainedItemIDs => _obtainedItemIDs;
    public HashSet<int> ClearPuzzleID => _clearPuzzleID;
    public HashSet<int>InteractObjects => _interactObjects; 
    public GameData SaveData
    {
        get => _gameData;
        set => _gameData = value;
    }

    public InventoryData Inventory => _inventoryData;
    public QuickSlot QuickSlot => _quickSlot;

    public int LatestChapter
    {
        get => _prevChapter;
        set => _prevChapter = value;
    }
    public int LatestStage
    {
        get => _prevStage;
        set => _prevStage = value;
    }
    public int HighestChapter
    {
        get => _gameData.HighestChapter;
        set => _gameData.HighestChapter = value;
    }
    public int HighestStage
    {
        get => _gameData.HighestStage;
        set => _gameData.HighestStage = value;
    }
    public int Chapter
    {
        get => _gameData.Chapter;
        set => _gameData.Chapter = value;
    }
    public int Stage
    {
        get => _gameData.Stage;
        set => _gameData.Stage = value;
    }

    public void Init()
    {
        _isPaused = false;
        
        _quickSlot = new QuickSlot();
        _inventoryData = new InventoryData();
        _inventorySaver = new InventorySaver(_inventoryData);
        _stageProgressSaver = new StageProgressSaver();
      
        _gameSaver = new GameSaver(_quickSlot);

    }
       
    
    
    public void SetResolutionMode(Resolution res)
    {
        Screen.SetResolution(res.width, res.height, SaveData.IsFullScreen);
        SaveData.SaveResolution = res;
       
    }

    public void SetScreenMode(bool value)
    {
        Screen.fullScreen = value;
        SaveData.IsFullScreen = value;
       
    }

    public void PauseGame()
    {
        if (!_isPaused)
        {
            _isPaused = true;
            Time.timeScale = 0;
            Debug.Log("Game paused.");
        }
    }

    public void ResumeGame()
    {
        if (_isPaused)
        {
            _isPaused = false;
            Time.timeScale = 1;
            Debug.Log("Game resumed.");
        }
    }

    public void ClearPuzzle(int id)
    {
        if (_clearPuzzleID.Add(id))
        {
            _stageProgressData.ClearedPuzzleIDs.Add(id);
            _stageProgressSaver.Save(_stageProgressData);
        }
    }

    public void ObtainItem(int mapItemId)
    {
        if (_obtainedItemIDs.Add(mapItemId))
        {
            _stageProgressData.ObtainedItemIDs = new List<int>(_obtainedItemIDs);
            _stageProgressSaver.Save(_stageProgressData);
        }
    }
    public void InteractedObjects(int id)
    {
        if(InteractObjects.Add(id))
        {
            _stageProgressData.InteractObjectIDS = new List<int>(_interactObjects);
            _stageProgressSaver.Save(_stageProgressData);
        }
    }
    public void CompleteStage()
    {
        if (Stage == Define.STAGES_PER_CHAPTER)
        {
            Chapter++;
            Stage = 1;
        }
        else
        {
            Stage++;
        }

        if (Chapter > HighestChapter || (Chapter == HighestChapter && Stage > HighestStage))
        {
            HighestChapter = Chapter;
            HighestStage = Stage;
        }

        SaveGame();
    }

    public void PrevStage()
    {
        if (Chapter == 1 && Stage == 1) return;

        Stage--;
        if (Stage == 0)
        {
            Chapter--;
            Stage = Define.STAGES_PER_CHAPTER;
        }
    }

    public void SaveGame()
    {
        _gameSaver.SaveGame(_gameData);
        _inventorySaver.SaveInventory();
        _stageProgressSaver.Save(_stageProgressData);
    
    }

    public bool LoadGame()
    {
        _gameSaver.LoadGame(ref _gameData);
        _inventorySaver.LoadInventory();
        _stageProgressData = _stageProgressSaver.Load();

        _clearPuzzleID = new HashSet<int>(_stageProgressData.ClearedPuzzleIDs);
        _obtainedItemIDs = new HashSet<int>(_stageProgressData.ObtainedItemIDs);
        _interactObjects = new HashSet<int>(_stageProgressData.InteractObjectIDS);

        return true;
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        Debug.Log("Game Quit.");
    }

    public bool HasFile()
    {
        return File.Exists(GameData.FilePath);
    }

    public void DeleteSaveData()
    {
        _gameSaver.DeleteData();
        _inventorySaver.DeleteData();
        _stageProgressSaver.DeleteData();
        _gameData = new GameData();
        Init();
    }
}
#endregion
