using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using System.Linq;

public interface ILoader<Key,value>
{
    Dictionary<Key, value> MakeDic(); //������ Dicitonary�� ����
    bool Validate();  //��ȿ�� �˻�
}

public class DataManager
{
    //public Dictionary<int, MonsterData> Monsters { get; private set; }
    public Dictionary<int, StageData> Stages { get; private set; }
    public Dictionary<int, ItemData> Items { get; private set; }
    public Dictionary<int, WeaponData> Weapons { get; private set; }
    public Dictionary<int, SpawnerData> Spawners { get; private set; }
    public Dictionary<int, PuzzleData> Puzzles { get; private set; }    
    public Dictionary<int, FieldItemData> FieldItems { get; private set; }
    public Dictionary<int,MonsterData> Monsters { get; private set; }
    public Dictionary<int, InteractObjectData> Interacts { get; private set; }  

    public Dictionary<int,ArmorData>Armors { get; private set; }


    public void Init(Action onComplete)
    {
        LoadJson<StageDataLoader, int, StageData>("StageData", (loader) => { Stages = loader.MakeDic(); });
        LoadJson<ItemDataLoader, int, ItemData>("ItemData", (loader) => { Items = loader.MakeDic(); });
        LoadJson<SpawnerDataLoader,int,SpawnerData>("SpawnData",(loader) => { Spawners = loader.MakeDic(); });
        LoadJson<PuzzleDataLoader, int, PuzzleData>("PuzzleData", (loader) => { Puzzles = loader.MakeDic(); });
        LoadJson<FieldItemDataLoader, int, FieldItemData>("FieldItemData", (loader) => { FieldItems = loader.MakeDic(); });
        LoadJson<MonsterDataLoader, int,MonsterData>("MonsterData", (loader) => { Monsters = loader.MakeDic(); });
        LoadJson<WeaponDataLoader, int, WeaponData>("WeaponData", (loader) => { Weapons = loader.MakeDic(); });
        LoadJson<InteractObjectDataLoader, int, InteractObjectData>("InteractData", (loader) => { Interacts = loader.MakeDic(); });
        LoadJson<ArmorDataLoader, int, ArmorData>("ArmorData", (loader) => { Armors = loader.MakeDic(); }); 

        onComplete?.Invoke();
    }
    public bool Loaded()
    {
        if (Monsters == null) return false;
        if (Weapons == null) return false;
        if (Interacts == null) return false;
        if (Stages == null) return false;
        if (Items == null) return false;
        if (Spawners == null) return false;
        if (Spawners == null) return false;
        if (Puzzles==null) return false;
        if (FieldItems == null) return false;
        if(Armors == null) return false;    
        return true;

        //if (Bosses == null)
        //    return false;
        
    }
        

       
       
        
    void LoadSingleJson<Value>(string key,Action<Value> callback)
    {
        Managers.Resource.LoadAsync<TextAsset>(key, (textAsset) =>
        {
            Value item = JsonUtility.FromJson<Value>(textAsset.text);
            callback.Invoke(item);
        });
    }
    void LoadJson<Loader, Key, Value>(string key, Action<Loader> callback) where Loader : ILoader<Key, Value>, new()
    {
        Managers.Resource.LoadAsync<TextAsset>(key, (textAsset) =>
        {
            if (textAsset == null)
            {
                Debug.LogError($"Failed to load JSON: {key}");
                return;
            }
           

            try
            {
                // JsonUtility�� ���� �ε�
                Loader wrapper = JsonUtility.FromJson<Loader>(textAsset.text);
                if (wrapper == null)
                {
                    Debug.LogError($"JsonUtility failed to parse: {key}");
                    return;
                }

                callback?.Invoke(wrapper);
                Debug.Log($"Successfully loaded JSON: {key}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error while parsing JSON from {key}: {ex.Message}");
            }
        });
    }

    void LoadSingleXml<Value>(string key, Action<Value> callback)
    {
        Managers.Resource.LoadAsync<TextAsset>(key, (textAsset) =>
        {
            XmlSerializer xs = new XmlSerializer(typeof(Value));
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
            {
                callback?.Invoke((Value)xs.Deserialize(stream));
            }
        });
    }

    void LoadXml<Loader, Key, Value>(string key, Action<Loader> callback) where Loader : ILoader<Key, Value>, new()
    {
        Managers.Resource.LoadAsync<TextAsset>(key, (textAsset) =>
        {
            XmlSerializer xs = new XmlSerializer(typeof(Loader));
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
            {
                callback?.Invoke((Loader)xs.Deserialize(stream));
            }
        });
    }
}
