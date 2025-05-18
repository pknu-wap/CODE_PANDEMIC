using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;
[Serializable]
public struct ToneData
{
    public Vector4 shadow;
    public Vector4 mid;
    public Vector4 highlight;
}

public class SDHController : MonoBehaviour
{
    private Volume _volume;

    private ShadowsMidtonesHighlights _smh;

    void Start()
    {
        _volume = GetComponent<Volume>();
        if (_volume.profile.TryGet(out _smh))
        {
            Debug.Log(_smh.shadows.value);
            _smh.active = false;
        }
        else
        {
            Debug.LogWarning("ShadowsMidtonesHighlights component not found in Volume Profile.");
        }
    }
    private void OnEnable()
    {
        Managers.Event.Subscribe("ToneUpdated",TotalUpdated );

    }
    private void OnDisable()
    {
        Managers.Event.Unsubscribe("ToneUpdated", TotalUpdated);
    }
   
    private void TotalUpdated(object obj)
    {
        if (obj is ToneData tone)
        {
            SettingTotalStageTone(tone);
        }
    }

    
    public void SettingStageShadowTone(Vector4 tone)
    {
        if (tone.x < 0) return;

       
        _smh.shadows.overrideState = true;
        _smh.shadows.value= tone;
       
    }
    public void SettingStageMidTone(Vector4 tone)
    {
        if (tone.x < 0) return;
      
        _smh.midtones.overrideState = true;
        _smh.midtones.value = tone;
    }
    public void SettingStageHighlightTone(Vector4 tone)
    {
        if (tone.x < 0) return;
       
        _smh.highlights.overrideState = true;
        _smh.highlights.value = tone;   
    }
    public void SettingTotalStageTone(ToneData tone)
    {
        if (_smh.active == false) _smh.active = true;
        SettingStageShadowTone(tone.shadow);
        SettingStageMidTone(tone.mid);
        SettingStageHighlightTone(tone.highlight);
        if(tone.shadow.x<0&&tone.mid.x<0&&tone.highlight.x<0)_smh.active = false;
    }
}
