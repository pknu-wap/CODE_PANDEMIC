using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Buff : UI_Base
{
    enum GameObjects
    {
        BG,
      
    }

    private Dictionary<int, UI_BuffSlot> _buffSlots = new Dictionary<int, UI_BuffSlot>();
    private Transform _slotParent;
   
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindObject(typeof(GameObjects));

        _slotParent = GetObject((int)GameObjects.BG).transform;
        RefreshBuffs();
      
        return true;
    }

    private void OnEnable()
    {
        Managers.Event.Subscribe("StatUpdated", Refresh);
    }

    private void OnDisable()
    {
        Managers.Event.Unsubscribe("StatUpdated", Refresh);
    }

    private void Refresh(object obj)
    {
        RefreshBuffs();
    }

    private void RefreshBuffs()
    {
        var activeBuffs = Managers.Buffs.ActiveBuffs;

        HashSet<int> activeBuffIds = new HashSet<int>();

        foreach (Buff buff in activeBuffs)
        {
            activeBuffIds.Add(buff.Data.TemplateID);

            if (_buffSlots.TryGetValue(buff.Data.TemplateID, out UI_BuffSlot slot))
            {
                slot.gameObject.SetActive(true);

               
                float ratio = Mathf.Clamp01(buff.TimeRemaining / buff.Data.Timer);
                slot.RefreshCoolTime(ratio);
            }
            else
            {
                // 신규 버프면 슬롯 생성
                Managers.Resource.Instantiate("UI_BuffSlot", _slotParent, (obj) =>
                {
                    UI_BuffSlot newSlot = obj.GetComponent<UI_BuffSlot>();
                    newSlot.Init();
                    newSlot.SetBuff(buff);
                    _buffSlots.Add(buff.Data.TemplateID, newSlot);
                });
            }
        }

       
        List<int> toRemove = new List<int>();

        foreach (var pair in _buffSlots)
        {
            if (activeBuffIds.Contains(pair.Key) == false)
            {
                
                Destroy(pair.Value.gameObject);
                toRemove.Add(pair.Key);
            }
        }

      
        foreach (int key in toRemove)
            _buffSlots.Remove(key);
    }

}
