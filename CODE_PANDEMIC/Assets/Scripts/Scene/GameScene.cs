using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    UI_GameScene _gameSceneUI;
    StageData _stageData;
    protected override bool Init()
    {
        if (base.Init() == false) return false;
        SceneType = Define.SceneType.GameScene;

        PrepareStage();
        return true;
    }
    public void PrepareStage()
    {
        StartCoroutine(CowaitLoad());
    }
       

    IEnumerator CowaitLoad()
    {
        Managers.UI.FadeIn();
        while (Managers.Data.Loaded() == false) yield return null;
        int templateID = (Managers.Game.Chapter - 1) * Define.STAGES_PER_CHAPTER + Managers.Game.Stage;
        if (Managers.Data.Stages.TryGetValue(templateID, out StageData stageData) == false) yield break;

        _stageData = stageData;
        Managers.UI.ShowSceneUI<UI_GameScene>(callback: (UI) =>
        {
            _gameSceneUI = UI;
        });
        Managers.Object.LoadStageData(_stageData);

        Managers.UI.FadeIn();
    }

    public void CompleteStage()
    {
        if(Managers.Game.Stage==Define.STAGES_PER_CHAPTER)
        {
            Managers.Game.Chapter++;
            Managers.Game.Stage = 1;
        }
        else
        {
            Managers.Game.Stage++;
        }
        PrepareStage();
    }
  
}
