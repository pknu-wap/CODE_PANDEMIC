using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatData
{
    public int MaxHp= 100;
    public float CurrentHp=-1;
    public int Defendence = 0;
    public float BaseSpeed = 3.5f;
    public float RunSpeed = 4.5f;
    public float DashSpeed = 8.0f;
    public float DashDuration = 0.3f;
    public float DashCoolDown = 0.5f;

}
public class StatModifier
{
    public int IncreaseHp = 0;
    public int IncreaseDefend = 0;
    public float IncreaseSpeed = 0.0f;
}
public class PlayerStat
{
    private StatData _statData;

   
    private Dictionary<int, ArmorData> _equippedArmors = new();

    public  PlayerStat()
    {
        _statData = new StatData();
    }
    public StatModifier TotalModifier
    {
        get
        {
            var modifier = new StatModifier();

        
            foreach (var armor in _equippedArmors.Values)
            {
                ArmorData data = armor;
                modifier.IncreaseHp += data.Health;
                modifier.IncreaseDefend += data.Defense;
                modifier.IncreaseSpeed += data.Speed;
               
               
            }

           
            if (Managers.Buffs != null)
            {
                foreach (var buff in Managers.Buffs.ActiveBuffs)
                {
                    StatModifier buffMod = buff.GetModifier();
                    modifier.IncreaseHp += buffMod.IncreaseHp;
                    modifier.IncreaseDefend += buffMod.IncreaseDefend;
                    modifier.IncreaseSpeed += buffMod.IncreaseSpeed;
                }
            }

            return modifier;
        }
    }
    

    public StatData StatData { get { return _statData; } private set { _statData = value; } }
   
    public int MaxHp => _statData.MaxHp + TotalModifier.IncreaseHp;
    public float CurrentHp => _statData.CurrentHp;
    public int Defend => _statData.Defendence + TotalModifier.IncreaseDefend;
    public float BaseSpeed => _statData.BaseSpeed + TotalModifier.IncreaseSpeed;
    public float RunSpeed => _statData.RunSpeed + TotalModifier.IncreaseSpeed;
    public float DashSpeed => _statData.DashSpeed;
    public float DashDuration => _statData.DashDuration;
    public float DashCoolDown => _statData.DashCoolDown;

    public void SetCurrentHp(float amount)
    {
        _statData.CurrentHp = amount;
        if (_statData.CurrentHp <= 0) _statData.CurrentHp = MaxHp;
    }

    public void UpdateCurrentHp(float amount)
    {
        _statData.CurrentHp += amount;
    }
    public void EquipArmor(ArmorData armor,int slot)
    {
        _equippedArmors[slot] = armor;
        Managers.Event.InvokeEvent("StatUpdated", this);
    }

    public void UnequipArmor(int slot)
    {
        _equippedArmors.Remove(slot);
        Managers.Event.InvokeEvent("StatUpdated", this);
    }

    public void LoadStatData(StatData data)
    {
        _statData = data;
    }
  
}
