using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    GameObject _fog;
    ParticleSystem _rain;
    private void OnEnable()
    {
        Managers.Event.Subscribe("RainUpdated",SettingRainEnvironment);
        Managers.Event.Subscribe("FogUpdated", SettingFogEnvironment);
    }
    private void OnDisable()
    {
        Managers.Event.Unsubscribe("RainUpdated", SettingRainEnvironment);
        Managers.Event.Unsubscribe("FogUpdated", SettingFogEnvironment);
    }
    private void SettingRainEnvironment(object obj)
    {
       if(obj is bool isRain)
        {
            if (!isRain)
            {
                if(_rain!=null)
                {
                    Destroy(_rain.gameObject);
                    _rain = null;
                }
                return;
            }
            if (_rain != null) return;

            Managers.Resource.Instantiate("RainEffect", transform, (obj) =>
            {
                _rain = obj.GetComponent<ParticleSystem>();
                obj.transform.localPosition = new Vector3(0, 6, 0);
            });
        }
    }

    private void SettingFogEnvironment(object obj)
    {
        if (obj is bool isFog)
        {
            if (!isFog)
            {
                if (_fog != null)
                {
                    Destroy(_fog.gameObject);
                    _fog = null;
                }
                return;
            }
            if (_fog != null) return;

            Managers.Resource.Instantiate("Fog", transform, (obj) =>
            {
                _fog = obj;
                _fog.transform.position = Vector3.zero;
            });
        }
    }

    

}
