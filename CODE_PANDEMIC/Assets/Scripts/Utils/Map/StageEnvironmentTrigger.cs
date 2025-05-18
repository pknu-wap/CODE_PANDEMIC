using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class StageEnvironmentTrigger : MonoBehaviour
{
   [SerializeField] StageEnvironmentData _environmentData;

    void Start()
    {
        if(_environmentData!=null) TriggerEnvironment();

    }

    void TriggerEnvironment()
    {
        Managers.Event.InvokeEvent("ToneUpdated", _environmentData.Tone);
        Managers.Event.InvokeEvent("RainUpdated", _environmentData.UseRain);
        Managers.Event.InvokeEvent("FogUpdated", _environmentData.UseFog);
     
    }
   
}
