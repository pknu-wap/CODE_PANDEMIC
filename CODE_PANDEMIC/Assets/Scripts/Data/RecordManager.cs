using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecordData
{
    public int ItemCount = 0;
    public int ZombieKillCount = 0;
    public int ClearPuzzleCount = 0;
    public int PlayerDeathCount = 0;
}
public class RecordManager 
{
    RecordData _recordData;
    RecordSaver _recordSaver;

    public int ItemCount => _recordData.ItemCount;
    public int ZombieKillCount => _recordData.ZombieKillCount;
    public int ClearPuzzleCount => _recordData.ClearPuzzleCount;
    public int PlayerDeathCount =>_recordData.PlayerDeathCount;

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

    public void Init()
    {
        _recordData = new RecordData();
        _recordSaver = new RecordSaver();
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

