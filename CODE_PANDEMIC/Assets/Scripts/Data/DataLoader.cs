using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[Serializable]
public class MonsterData
{
    public int TemplateID;
    public string NameID;
    public string Prefab;
    public int Hp;
    public int AttackDamage;
    public int moveSpeed;
    public int AttackRange;
  
}
[Serializable]
public class MonsterDataLoader:ILoader<int, MonsterData>
{
    public List<MonsterData> monsters = new List<MonsterData>();
public Dictionary<int, MonsterData> MakeDic()
    {
        Dictionary<int, MonsterData> dic = new Dictionary<int, MonsterData>();
        foreach (MonsterData monster in monsters)
            dic.Add(monster.TemplateID, monster);
        return dic;
    }

    public bool Validate()
    {
        return true;
    }
}
[Serializable]
public class StageData
{
    public int TemplateID;
    public int ChapterID;
    public int StageID;
    public int BossTemplateID;
    public string MapName;
    
}
[Serializable]
public class StageDataLoader : ILoader<int, StageData>
{
    public List<StageData> stages = new List<StageData>();
    public Dictionary<int, StageData> MakeDic()
    {
        Dictionary<int, StageData> dic = new Dictionary<int, StageData>();
        foreach (StageData stage in stages)
            dic.Add(stage.TemplateID, stage);
        return dic;
    }
    public bool Validate()
    {
        return true;
    }
}
namespace Inventory.Model
{
    [Serializable]
    public class ItemData
    {
        public int TemplateID;
        public string Name;
        public string Description;
        public bool IsStackable;
        public int MaxStackSize;
        public string Sprite;
        public ItemType Type;
        public WeaponType Weapon;
        public List<ItemParameter> Parameters;
        public string GetParameterInfo(int index)
        {
            if (index >= 0 && index < Parameters.Count)
            {
                return $"{Parameters[index].parameterName} : {Parameters[index].value}";
            }
            return string.Empty;
        }
    }
    [Serializable]
    public class ItemDataLoader : ILoader<int, ItemData>
    {
        public List<ItemData> items = new List<ItemData>();
        public Dictionary<int, ItemData> MakeDic()
        {
            Dictionary<int, ItemData> dic = new Dictionary<int, ItemData>();
            foreach (ItemData stage in items)
                dic.Add(stage.TemplateID, stage);
            return dic;
        }
        public bool Validate()
        {
            return true;
        }
    }

    [Serializable]
    public class ItemParameter
    {
        public string parameterName;
        public float value;
    }
}
[Serializable]
public class InventoryItemData
{
    public int ItemID;
    public int Quantity;
    public List<ItemParameter> ItemState;
}

[Serializable]
public class InventorySaveData
{
    public List<InventoryItemData> InventoryItems;
}