using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


[Serializable]
public class RecordData
{
    public int ItemCount = 0;
    public int ZombieKillCount = 0;
    public int ClearPuzzleCount = 0;
    public int PlayerDeathCount = 0;
    public InteractRecordData InteractData = new();
}

[Serializable]
public class PlayerRecordData
{
    public int ItemCount = 0;
    public int ZombieKillCount = 0;
    public int ClearPuzzleCount = 0;
    public int PlayerDeathCount = 0;
}

public class RecordManager 
{
    PlayerRecordData _recordData;
    RecordSaver _recordSaver;

    InteractTutorialMapSO _interactTutorialMap;
    InteractRecordData _interactRecordData = new();

    public int ItemCount => _recordData.ItemCount;
    public int ZombieKillCount => _recordData.ZombieKillCount;
    public int ClearPuzzleCount => _recordData.ClearPuzzleCount;
    public int PlayerDeathCount =>_recordData.PlayerDeathCount;
    public void Init()
    {
        _recordData = new PlayerRecordData();
        _recordSaver = new RecordSaver();
        if (_interactTutorialMap == null)
        {
            Managers.Resource.LoadAsync<InteractTutorialMapSO>("InteractTutorialData", (obj) =>
            {
                _interactTutorialMap = obj;
            });
        }
    }
    public void AddItemCount()
    {
        _recordData.ItemCount++;
        if (ItemCount == 1)
        {
            Managers.UI.ShowPopupUI<UI_TutorialPopUp>("UI_ItemTutorialPopUp");
        }
    }
   
    public void AddZombieKillCount()
    {
        _recordData.ZombieKillCount++;
        if (ZombieKillCount == 1)
        {
            Managers.UI.ShowPopupUI<UI_TutorialPopUp>("UI_ZombieTutorialPopUp");
        }
    }
    public void AddClearPuzzleCount()
    {
        _recordData.ClearPuzzleCount++;
    }
    public void AddPlayerDeathCount()
    {
        _recordData.PlayerDeathCount++;
    }
    public void AddInteractCount(InteractType type)
    {
        _interactRecordData.Add(type);

        if (_interactRecordData.Get(type) == 1)
        {
            if (_interactTutorialMap != null)
            {
                string popupName = _interactTutorialMap.GetPopupName(type);

                if (!string.IsNullOrEmpty(popupName))
                {
                    Managers.UI.ShowPopupUI<UI_TutorialPopUp>(popupName);
                }
            }
            else
            {
                Debug.LogWarning("InteractTutorialMapSO is not yet loaded. Popup skipped.");
            }
        }
    }




    public void LoadData()
    {        
       _recordData= _recordSaver.LoadRecord();
    }
    public void SaveData()
    {
        _recordSaver.SaveRecord(_recordData);
    }
    public void ResetData()
    {
        _recordSaver.ResetRecord();
    }
}

