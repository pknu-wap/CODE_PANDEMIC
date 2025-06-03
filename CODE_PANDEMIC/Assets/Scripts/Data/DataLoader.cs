using Inventory.Model;
using Inventory.Model;
using System;
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
    public float AttackDelay;
    public float DetectionRange;
    public float DetectionAngle;
    public float MoveSpeed;
    public float AttackRange;
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
public class BossData
{
    public int TemplateID;
    public string NameID;
    public string Prefab;
    public int Hp;
    public int AttackDamage;
    public float MoveSpeed;
   
}
[Serializable]
public class BossDataLoader : ILoader<int, BossData>
{
    public List<BossData> bossMonsters = new List<BossData>();
    public Dictionary<int, BossData> MakeDic()
    {
        Dictionary<int, BossData> dic = new Dictionary<int, BossData>();
        foreach (BossData boss in bossMonsters)
            dic.Add(boss.TemplateID, boss);
        return dic;
    }

    public bool Validate()
    {
        return true;
    }
}

[Serializable]
public class SpawnerInfoData
{
    public int ID;
    public string Name;
    public Vector2 Pos;
}
[Serializable]
public class StageData
{
    public int TemplateID;
    public int ChapterID;
    public int StageID;
    public int BossTemplateID;
    public string MapAddress;
    public string MapName;
    public List<SpawnerInfoData> Spawners;
    public List<int> Puzzles;
    public List<int> FieldItems;
    public List<int> InteractableObjects;
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

[Serializable]
public class MonsterPosData
{
    public int ID;
    public Vector2 Pos;
}

[Serializable]
public class SpawnerData
{
    public int TemplateID;
    public List<MonsterPosData> Monsters;
}
[Serializable]
public class SpawnerDataLoader : ILoader<int, SpawnerData>
{
    public List<SpawnerData> spawners = new List<SpawnerData>();
    public Dictionary<int, SpawnerData> MakeDic()
    {
        Dictionary<int, SpawnerData> dic = new Dictionary<int, SpawnerData>();
        foreach (SpawnerData spawner in spawners)
            dic.Add(spawner.TemplateID, spawner);
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
        public List<ItemParameter> Parameters;
        public WeaponType Weapon;
        public string GetParameterInfo(int index)
        {
            if (index >= 0 && index < Parameters.Count)
            {
                string color = Parameters[index].parameterName == "Health" ? "#00FFFF" : "#FF4444";
                return $"{Parameters[index].parameterName} : <color={color}>{Parameters[index].value}</color>";
              
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


[Serializable]
public class WeaponData
{
    public int TemplateID;
    public int Damage; //damage
    public float FireRate; //firerate
    public float BulletSpeed;
    public float Range;
    public float ReloadTime;
    public float SpreadAngle;
    public string WeaponPrefab;
    public int BulletID;
    public int BulletCount;
    public WeaponType Type;
}
public class WeaponDataLoader : ILoader<int, WeaponData>
{
    public List<WeaponData> weapons = new List<WeaponData>();
    public Dictionary<int, WeaponData> MakeDic()
    {
        Dictionary<int, WeaponData> dic = new Dictionary<int, WeaponData>();
        foreach (WeaponData weapon in weapons)
            dic.Add(weapon.TemplateID, weapon);
        return dic;
    }
    public bool Validate()
    {
        return true;
    }
}

[Serializable]
public class ArmorData
{
    public int TemplateID;
    public int Defense;
    public int Health;
    public float Speed;
    public ArmorType Type;

}
public class ArmorDataLoader : ILoader<int, ArmorData>
{
    public List<ArmorData> armors= new List<ArmorData>();
    public Dictionary<int, ArmorData> MakeDic()
    {
        Dictionary<int, ArmorData> dic = new Dictionary<int, ArmorData>();
        foreach (ArmorData weapon in armors)
            dic.Add(weapon.TemplateID, weapon);
        return dic;
    }
    public bool Validate()
    {
        return true;
    }
}
[Serializable]
public class BuffItemData
{
    public int TemplateID;
    public int IncreaseHealth;
    public int IncreaseDefend;
    public float IncreaseSpeed;
    public float Timer;
}
[Serializable]
public class BuffItemDataLoader : ILoader<int, BuffItemData>
{
    public List<BuffItemData> buffItems = new List<BuffItemData>();
    public Dictionary<int, BuffItemData> MakeDic()
    {
        Dictionary<int, BuffItemData> dic = new Dictionary<int, BuffItemData>();
        foreach (BuffItemData buff in buffItems)
            dic.Add(buff.TemplateID, buff);
        return dic;
    }

    public bool Validate()
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class RewardData
{
    public int ID;
    public int Quantity;
}
[Serializable]
public class BlockData
{
    public Vector3 Pos;
    public Vector2 Offset;
    public Vector2 Size;
    public string Prefab; 
}

[Serializable]
public class PuzzleData
{
    public int ID;
    public string Prefab;
    public string UIPath=null;
    public RewardData RewardItem;
    public Vector3 Pos;
    public bool IsMain;
    public BlockData LinkedBlock;
    public int RememberCount;
}    

[Serializable]
public class PuzzleDataLoader : ILoader<int, PuzzleData>
{
    public List<PuzzleData> puzzles = new List<PuzzleData>();
    public Dictionary<int, PuzzleData> MakeDic()
    {
        Dictionary<int, PuzzleData> dic = new Dictionary<int, PuzzleData>();
        foreach (PuzzleData puzzle in puzzles)
            dic.Add(puzzle.ID, puzzle);
        return dic;
    }
    public bool Validate()
    {
        return true;
    }
}

[Serializable]
public class PlayerData
{
    public int MaxHp;
    public int CurrentHp;
    public int Speed;
    public int SpeedMultiplier;
}
[Serializable]
public class FieldItemData
{
    public int ID;
    public int ItemID;
    public Vector3 Pos;
}
[Serializable]
public class FieldItemDataLoader : ILoader<int, FieldItemData>
{
    public List<FieldItemData> fieldItems = new List<FieldItemData>();
    public Dictionary<int, FieldItemData> MakeDic()
    {
        Dictionary<int, FieldItemData> dic = new Dictionary<int, FieldItemData>();
        foreach (FieldItemData fieldItem in fieldItems)
            dic.Add(fieldItem.ID, fieldItem);
        return dic;
    }
    public bool Validate()
    {
        return true;
    }
}
[Serializable]
public class InteractObjectData
{
    public int ID;
    public Vector3 Pos;
    public string Prefab;
    public int KeyID;
    public List<RewardData> Rewards;
}
[Serializable]
public class InteractObjectDataLoader : ILoader<int, InteractObjectData>
{
    public List<InteractObjectData> objects = new List<InteractObjectData>();
    public Dictionary<int, InteractObjectData> MakeDic()
    {
        Dictionary<int, InteractObjectData> dic = new Dictionary<int, InteractObjectData>();
        foreach (InteractObjectData obj in objects)
            dic.Add(obj.ID, obj);
        return dic;
    }
    public bool Validate()
    {
        return true;
    }
}
