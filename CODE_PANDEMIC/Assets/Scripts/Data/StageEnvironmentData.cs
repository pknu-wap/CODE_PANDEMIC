using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Stage/Environment Data", fileName = "NewStageEnvironment")]
public class StageEnvironmentData : ScriptableObject
{
    public ToneData Tone;
    public bool UseRain;
    public bool UseFog;
}
