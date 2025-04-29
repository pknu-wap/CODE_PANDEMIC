using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : BaseScene
{
    UI_GameScene _gameSceneUI;
    StageData _stageData;
    private int _prevStage;
    private int _prevChapter;
    private bool _uiLoad;
    protected override bool Init()
    {
        if (base.Init() == false) return false;
        SceneType = Define.SceneType.GameScene;
        Managers.UI.FadeAtOnce();
        _uiLoad = false;
        PrepareStage();
        return true;
    }
    public void PrepareStage()
    {
       
        StartCoroutine(CowaitLoad());
    }
    private  void OnEnable()
    {
        Managers.Event.Subscribe("OnPlayerDead", OnPlayerDead);
        Managers.Event.Subscribe("NextStage", NextStage);
        Managers.Event.Subscribe("PrevStage", PrevStage);
    }
    private void OnDisable()
    {
        Managers.Event.Unsubscribe("OnPlayerDead", OnPlayerDead);
        Managers.Event.Unsubscribe("NextStage", NextStage);
        Managers.Event.Unsubscribe("PrevStage", PrevStage);


    }


    IEnumerator CowaitLoad()
    {
        while (Managers.Data.Loaded() == false) yield return null;
        int templateID = (Managers.Game.Chapter - 1) * Define.STAGES_PER_CHAPTER + Managers.Game.Stage;
        if (Managers.Data.Stages.TryGetValue(templateID, out StageData stageData) == false) yield break;
        
        _stageData = stageData;
        if (_gameSceneUI == null)
        {
            Managers.UI.ShowSceneUI<UI_GameScene>(callback: (UI) =>
            {
                _gameSceneUI = UI;
                _uiLoad = true;
            });
        }
        while (!_uiLoad)yield return null;
        StartCoroutine(Managers.Object.CoLoadStage(stageData));
        while (Managers.Object.Loaded == false) yield return null;
        
        Managers.UI.FadeIn();
    }
    private void ChangeStage()
    {
        Managers.UI.FadeOut(() => StartCoroutine(CoChangeStage()));
      
    }

    private IEnumerator CoChangeStage()
    {
        Managers.Object.ResetStage();
        yield return null; // Destroy 완료될 때까지 1프레임 대기
        PrepareStage();
    }
    private void UpdateLatestStage()
    {
        Managers.Game.LatestChapter= Managers.Game.Chapter;
        Managers.Game.LatestStage= Managers.Game.Stage;
    }
    public void NextStage(object  obj)
    {
        UpdateLatestStage();
        Managers.Game.CompleteStage();
        ChangeStage();
    }
   
    public void PrevStage(object obj)
    {
        UpdateLatestStage();
        Managers.Game.PrevStage();
        ChangeStage();
    }
    private void OnPlayerDead(object obj)
    {
        
        ChangeStage();
    }
        
}