
using System.Collections;
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
    private void OnDisable()
    {
        Managers.Event.Unsubscribe("Cinematic", OnCinematic);
        Managers.Event.Unsubscribe("EndCinematic", OnEndCinematic);
    }
    private void OnEndCinematic(object obj)
    {
        _camera.Priority = Define.None;
        if (obj is Define.CinematicType type)
        {
            switch (type)
            {
                case Define.CinematicType.PuzzleClear:
                    StartCoroutine(EndPuzzleCinematic());
                    break;
                case Define.CinematicType.BossSequence:
                    _camera.Priority= Define.None; //smooth step
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

  
}
