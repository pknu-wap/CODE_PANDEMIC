using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuffSlot : UI_Base
{
    enum Images
    {
        BuffImage,
        CoolTimeImage
    }

    Image _buffImage;
    Image _coolTimeImage;
    private float _totalCoolTime = -1f;
    private Buff _buff;
    private Coroutine _coUpdate;

    public override bool Init()
    {
        if (base.Init() == false) return false;
        _totalCoolTime = -1;
        BindImage(typeof(Images));
        _buffImage = GetImage((int)Images.BuffImage);
        _coolTimeImage = GetImage((int)Images.CoolTimeImage);
        return true;
    }

    public void SetBuff(Buff buff)
    {
        _buff = buff;

        if (_totalCoolTime == -1f)
        {
            _totalCoolTime = _buff.TimeRemaining;

            if (Managers.Data.Items.TryGetValue(buff.Data.TemplateID, out ItemData data))
            {
                Managers.Resource.LoadAsync<Sprite>(data.Sprite, (obj) =>
                {
                    SettingNewBuff(obj);
                });
            }

           
            if (_coUpdate != null)
                StopCoroutine(_coUpdate);
            _coUpdate = StartCoroutine(CoUpdateCoolTime());
        }
        else
        {
            RefreshCoolTime(0);
        }
            
    }

    private IEnumerator CoUpdateCoolTime()
    {
        while (_buff != null)
        {
            float ratio = Mathf.Clamp01(_buff.TimeRemaining / _totalCoolTime);
            RefreshCoolTime(ratio);

          
            if (ratio <= 0.2f)
            {
                float flash = Mathf.PingPong(Time.time * 5f, 1f);
                _buffImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(0.3f, 1f, flash));
                float a = _coolTimeImage.color.a;
              
            }
            else
            {
                _buffImage.color = Color.white;
            }

            if (_buff.TimeRemaining <= 0)
            {
                gameObject.SetActive(false);
                yield break;
            }

            yield return CoroutineHelper.WaitForSeconds(0.2f);
        }
    }

    public void RefreshCoolTime(float val)
    {
        _coolTimeImage.fillAmount = 1 - val;
    }

    void SettingNewBuff(Sprite sprite)
    {
        _buffImage.sprite = sprite;
        RefreshCoolTime(1f);
    }
}
