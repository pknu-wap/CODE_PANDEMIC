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
    Dictionary<Key, value> MakeDic(); //데이터 Dicitonary로 만듬
    bool Validate();  //유효성 검사
}

public class DataManager
{
    //public Dictionary<int, MonsterData> Monsters { get; private set; }
    public Dictionary<int, StageData> Stages { get; private set; }
    public Dictionary<int, ItemData> Items { get; private set; }
   
    public Dictionary<int, WeaponData> Weapons { get; private set; }
    public void Init(Action onComplete)
    {
        LoadJson<StageDataLoader, int, StageData>("StageData", (loader) => { Stages = loader.MakeDic(); });
        LoadJson<ItemDataLoader, int, ItemData>("ItemData", (loader) => { Items = loader.MakeDic(); });
        onComplete?.Invoke();
    }
    public bool Loaded()
    {
        //if (Monsters == null)
        //    return false;
        //if (Weapons == null)
        //    return false;
        if (Stages == null)
            return false;
        if (Items == null)
            return false;
        //if (Bosses == null)
        //    return false;
        return true;
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
                // JsonUtility로 직접 로드
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
