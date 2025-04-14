using System.Collections;

using UnityEngine;

public class GameScene : BaseScene
{
    UI_GameScene _gameSceneUI;
    StageData _stageData;
    protected override bool Init()
    {
        if (base.Init() == false) return false;
        SceneType = Define.SceneType.GameScene;
        Managers.UI.FadeAtOnce();
        PrepareStage();
        return true;
    }
    public void PrepareStage()
    {
        StartCoroutine(CowaitLoad());
    }


    IEnumerator CowaitLoad()
    {
        while (Managers.Data.Loaded() == false) yield return null;
        int templateID = (Managers.Game.Chapter - 1) * Define.STAGES_PER_CHAPTER + Managers.Game.Stage;
        if (Managers.Data.Stages.TryGetValue(templateID, out StageData stageData) == false) yield break;
        
        _stageData = stageData;
        Managers.UI.ShowSceneUI<UI_GameScene>(callback: (UI) =>
        {
            _gameSceneUI = UI;

        });

        StartCoroutine(Managers.Object.CoLoadStageData(stageData));
        while (Managers.Object.Loaded == false) yield return null;
       
        Managers.UI.FadeIn();
    }
    private void ChangeStage()
    {
        Managers.UI.FadeOut();
        Managers.Object.ResetStage();
        PrepareStage();
    }

    public void NextStage()
    {
        Managers.Game.CompleteStage();
        ChangeStage();
    }
  
    public void PrevStage()
    {
        Managers.Game.PrevStage();
        ChangeStage();
    }
}