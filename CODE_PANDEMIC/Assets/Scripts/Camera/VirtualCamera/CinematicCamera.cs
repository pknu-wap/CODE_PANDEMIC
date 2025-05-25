
using System.Collections;
using UnityEngine;

public class CinematicCamera :VirtualCameraBase
{
    protected override bool Init()
    {
        if (base.Init()==false) return false;
        _camera.Priority = Define.None;
        _camera.gameObject.SetActive(false);
        return true;
    }
    public void OnCinematic()
    {
        _camera.Priority = Define.Cinematic;
    }

    public void OnEndCinematic(Define.CinematicType type)
    {
        _camera.Priority = Define.None;
       
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
    IEnumerator EndPuzzleCinematic()
    {
        Managers.UI.FadeAtOnce();
        yield return CoroutineHelper.WaitForSeconds(0.5f);
        Managers.UI.FadeIn();
    }
  
  
}
