using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CinematicCamera :VirtualCameraBase
{

    protected override bool Init()
    {
        if (base.Init()==false) return false;
        _camera.Priority = Define.None;
        return true;
    }
    private void OnEnable()
    {
        Managers.Event.Subscribe("Cinematic", OnCinematic);
        Managers.Event.Subscribe("EndCinematic", OnEndCinematic);
    }

    private void OnEndCinematic(object obj)
    {
        _camera.Priority = Define.None;
       if(obj is  Define.CinematicType type)
       {
            switch(type)
            {
                case Define.CinematicType.PuzzleClear:
                    StartCoroutine(EndPuzzleCinematic());
                    break;
                case Define.CinematicType.BossSequence: //자연스럽게 돌아가게 
                    break;
            }
       }
    }
    
    IEnumerator EndPuzzleCinematic()
    {
        Managers.UI.FadeAtOnce();
        yield return CoroutineHelper.WaitForSeconds(0.5f);
        Managers.UI.FadeIn();
    }
    private void OnCinematic(object obj)
    {
        _camera.Priority = Define.Cinematic;
    }

    private void OnDisable()
    {
        Managers.Event.Unsubscribe("Cinematic", OnCinematic);
        Managers.Event.Unsubscribe("EndCinematic", OnEndCinematic);
    }

}
