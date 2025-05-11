using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Buff
{
    private BuffItemData _data;

    private float _timeRemaining;
    public BuffItemData Data => _data;
    public float TimeRemaining => _timeRemaining;
    public Buff(int ID)
    {
        _timeRemaining = 0;
        if (Managers.Data.BuffItems.TryGetValue(ID, out BuffItemData data))
        {
            _data = data;
            _timeRemaining = data.Time;
        }
    }
    public StatModifier GetModifier()
    {
        return new StatModifier
        {
            IncreaseHp = _data.IncreaseHealth,
            IncreaseDefend = _data.IncreaseDefend,
            IncreaseSpeed = _data.IncreaseSpeed
        };
    }

    public void Refresh()
    {
        _timeRemaining = _data.Time;
    }
    public void Tick(float deltaTime)
    {
        _timeRemaining -= deltaTime;
        if (_timeRemaining <= 0f)
        {
            _timeRemaining = 0f;
            Expire();
        }

    }

    private void Expire()
    {
        Managers.Event.InvokeEvent("OnBuffEnded", _data);
    }


}
public class BuffManager : MonoBehaviour
{
    private List<Buff> _activeBuffs = new List<Buff>();
    public List<Buff> ActiveBuffs => _activeBuffs;


    private void Update()
    {
        for (int i = _activeBuffs.Count - 1; i >= 0; i--)
        {
            _activeBuffs[i].Tick(Time.deltaTime);

            if (_activeBuffs[i].TimeRemaining <= 0)
            {
                _activeBuffs.RemoveAt(i);
                Managers.Event.InvokeEvent("StatUpdated");
            }
        }
    }

    public void AddBuff(int ID)
    {
        Buff existingBuff = _activeBuffs.Find(buff => buff.Data.TemplateID == ID);
        if (existingBuff != null)
        {
            existingBuff.Refresh();
        }
        else
        {
            Buff newBuff = new Buff(ID);
            _activeBuffs.Add(newBuff);
        }
        Managers.Event.InvokeEvent("StatUpdated");
    }

}