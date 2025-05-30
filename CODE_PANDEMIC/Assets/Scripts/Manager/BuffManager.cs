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
           
            _timeRemaining = data.Timer;
            
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
        _timeRemaining = _data.Timer;
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
    public void ClearBuff()
    {
        if (_timeRemaining <= 0f) return;
        _timeRemaining = 0f;
        Expire();
    }
    private void Expire()
    {
        Managers.Event.InvokeEvent("OnBuffEnded", _data);
    }


}
public class BuffManager : MonoBehaviour
{
    private Dictionary<int, Buff> _activeBuffs = new Dictionary<int, Buff>();
    public IEnumerable<Buff> ActiveBuffs => _activeBuffs.Values;

    private void Update()
    {
        List<int> expiredBuffs = new List<int>();

        foreach (var pair in _activeBuffs)
        {
            pair.Value.Tick(Time.deltaTime);
            if (pair.Value.TimeRemaining <= 0)
                expiredBuffs.Add(pair.Key);
        }

        if (expiredBuffs.Count > 0)
        {
            foreach (int id in expiredBuffs)
            {
                _activeBuffs.Remove(id);
            }
            Managers.Event.InvokeEvent("StatUpdated");
        }
    }

    public void AddBuff(int ID)
    {
        if (_activeBuffs.TryGetValue(ID, out Buff existingBuff))
        {
            existingBuff.Refresh();
        }
        else
        {
            Buff newBuff = new Buff(ID);
            _activeBuffs.Add(ID, newBuff);
        }

        Managers.Event.InvokeEvent("StatUpdated");
    }
    public void ClearAllBuffs()
    {
        foreach (var buff in _activeBuffs.Values)
        {
            buff.ClearBuff();
        }
        _activeBuffs.Clear();
        Managers.Event.InvokeEvent("StatUpdated");
    }
}
