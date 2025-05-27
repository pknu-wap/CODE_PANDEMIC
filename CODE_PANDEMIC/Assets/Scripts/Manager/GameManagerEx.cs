using Inventory.Model;
using Inventory.Model.Inventory.Model;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

#region GameData 구조체 정의


[Serializable]
public class GameData
{
    public int HighestChapter = 1;
    public int HighestStage = 1;
    public int Chapter =1;
    public int Stage = 1;

    public int LatestChapter = 0;
    public int LatestStage = 0;  

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
    private bool _isPaused;
    #region Slot

    private QuickSlot _quickSlot;
    private QuickSlotSaver _quickSlotSaver;
    
    private EquipSlot _equipSlot;
    private EquipSaver _equipSaver;
    #endregion
    #region Data
    private GameData _gameData = new GameData();
    private GameSaver _gameSaver;
    
    private InventoryData _inventoryData;
    private InventorySaver _inventorySaver;
    
    private StageProgressData _stageProgressData;
    private StageProgressSaver _stageProgressSaver;

    private PlayerStat _playerStat;
    private PlayerStatSaver _playerStatSaver;

    private RecordManager _record;
    private HashSet<int> _clearPuzzleID;
    private HashSet<int> _obtainedItemIDs;
    private HashSet<int> _interactObjects;

    #endregion

    

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
    public EquipSlot EquipSlot => _equipSlot;
    public PlayerStat PlayerStat=> _playerStat;

    #region Chapter&Stage
    public int LatestChapter
    {
        get => _gameData.LatestChapter;
        set => _gameData.LatestChapter = value;
    }

    #region RecordData
    public void AddItemCount() => _record.AddItemCount();
    public void AddZombieKillCount()=>_record.AddZombieKillCount();
    public void AddPlayerDeathCount() => _record.AddPlayerDeathCount();
    public void AddClearPuzzleCount() => _record.AddClearPuzzleCount();
    public void AddInteractCount(Define.InteractType type) => _record.AddInteractCount(type);
    #endregion

    public int LatestStage
    {
        get => _gameData.LatestStage;
        set => _gameData.LatestStage = value;
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

    #endregion

    public void Init()
    {
        _isPaused = false;

        _quickSlot = new QuickSlot();
        _quickSlotSaver =new  QuickSlotSaver(QuickSlot);

        _equipSlot = new EquipSlot();
        _equipSaver = new EquipSaver(_equipSlot);

        _playerStat = new PlayerStat();
        _playerStatSaver = new PlayerStatSaver(_playerStat.StatData);
        
        _inventoryData = new InventoryData();
        _inventorySaver = new InventorySaver(_inventoryData);

        _stageProgressSaver = new StageProgressSaver();
        _gameSaver = new GameSaver();

        _record = new RecordManager();
        _record.Init();

        _clearPuzzleID = new HashSet<int>();
        _obtainedItemIDs = new HashSet<int>();
        _interactObjects = new HashSet<int>();
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
    public bool IsNextStage()
    {
        if (Chapter > LatestChapter) return true;
        else if (Chapter < LatestChapter) return false;
        else
        {
            if (Stage > LatestStage) return true;
            else return false;
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
    public void UpdateLatestStage()
    {
        LatestChapter = Chapter;
       LatestStage = Stage;
    }
    public void SaveGame()
    {
        _gameSaver.SaveGame(_gameData);
        _inventorySaver.SaveInventory();
        _playerStatSaver.SaveStat();
        _equipSaver.SaveEquip();
        _record.SaveData();
        _quickSlotSaver.SaveQuickSlot();
        _stageProgressSaver.Save(_stageProgressData);

    }

    public bool LoadGame()
    {
        _gameSaver.LoadGame(ref _gameData);

        _inventorySaver.LoadInventory();

        _stageProgressData = _stageProgressSaver.Load();

        _equipSaver.LoadEquip();
        _quickSlotSaver.LoadQuickSlot();

        _record.LoadData();

        _playerStat.LoadStatData(_playerStatSaver.LoadStat());
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
        _quickSlotSaver.DeleteData();
        _equipSaver.DeleteData();
        _playerStatSaver.DeleteData();
        _record.ResetData();

        _gameData = new GameData();
        Init();
    }
}
#endregion
