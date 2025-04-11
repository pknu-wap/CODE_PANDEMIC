using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_CoolTime : UI_Base
{
    enum Texts
    {
        Timer,
    }
    enum Images
    {
        Disable
    }
    float _coolTime;
 
    Image _disable;
    TextMeshProUGUI _timer;

    public override bool Init()
    {
        if (base.Init() == false) return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        _disable = GetImage((int)(Images.Disable));
        _timer = GetText((int)(Texts.Timer));
        Initialize();
        return true;
    }
    private void Initialize()
    {
        _disable.fillAmount = 0f;
        _timer.text = "";
        StartCoolTime(10.0f);

    }
    public void StartCoolTime(float maxTime)
    {
        StartCoroutine(CoolTimeRoutine(maxTime));
    }

    IEnumerator CoolTimeRoutine(float maxTime)
    {
        float time = maxTime;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            _disable.fillAmount = time / maxTime;
            _timer.text = Mathf.CeilToInt(time).ToString();
            yield return null;
        }
        _disable.fillAmount = 0f;
        _timer.text = "";
    }


}
