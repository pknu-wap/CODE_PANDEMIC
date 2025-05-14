using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;
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
            _smh.active = false;
        }
        else
        {
            Debug.LogWarning("ShadowsMidtonesHighlights component not found in Volume Profile.");
        }
    }
    private void OnEnable()
    {
        Managers.Event.Subscribe("ShadowToneUpGraded", ShadowToneUpdated);
        Managers.Event.Subscribe("MidToneUpGraded", MidToneUpdated);
        Managers.Event.Subscribe("StageHighlightUpGraded", StageHightlightUpdated);
        Managers.Event.Subscribe("TotalUpgraded",TotalUpdated );

    }
    private void OnDisable()
    {
        Managers.Event.Unsubscribe("ShadowToneUpGraded", ShadowToneUpdated);
        Managers.Event.Unsubscribe("MidToneUpGraded", MidToneUpdated);
        Managers.Event.Unsubscribe("StageHighlightUpGraded", StageHightlightUpdated);
        Managers.Event.Unsubscribe("TotalUpgraded", TotalUpdated);
    }
    private void ShadowToneUpdated(object obj)
    {
        if (obj is Vector4 tone)
        {
            SettingStageShadowTone(tone);
        }
    }
    private void MidToneUpdated(object obj)
    {
        if (obj is Vector4 tone)
        {
            SettingStageMidTone(tone);
        }
    }
    private void StageHightlightUpdated(object obj)
    {
        if (obj is Vector4 tone)
        {
            SettingStageHighlightTone(tone);
        }
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
        if(_smh.active==false)_smh.active=true;
        _smh.shadows.overrideState = true;
        _smh.shadows.value= tone;
       
    }
    public void SettingStageMidTone(Vector4 tone)
    {
        if (_smh.active == false) _smh.active = true;
        _smh.midtones.overrideState = true;
        _smh.midtones.value = tone;
    }
    public void SettingStageHighlightTone(Vector4 tone)
    {
        if (_smh.active == false) _smh.active = true;
        _smh.highlights.overrideState = true;
        _smh.highlights.value = tone;   
    }
    public void SettingTotalStageTone(ToneData tone)
    {
        if (_smh.active == false) _smh.active = true;
        SettingStageShadowTone(tone.shadow);
        SettingStageMidTone(tone.mid);
        SettingStageHighlightTone(tone.highlight);
    }
}
