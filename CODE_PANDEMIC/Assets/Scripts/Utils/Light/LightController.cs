using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    [SerializeField] Color _originColor;
    Light2D _light;
    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _light.color = _originColor;
    }
  
    public void SettingLight(Color color)
    {
        Color lightColor = color;

        _light.color = lightColor;
    }
}
