using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public interface ILoader<Key,value>
{
    Dictionary<Key, value> MakeDic(); //데이터 Dicitonary로 만듬
    bool Validate();  //유효성 검사
}

public class DataManager : MonoBehaviour
{
    public void Init()
    {

    }
    public bool Loaded()
    {
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
    void LoadJson<Loader,Key,Value>(string key,Action<Loader>callback)where Loader:ILoader<Key,Value>
    {
        Managers.Resource.LoadAsync<TextAsset>(key, (textAsset) =>
        {
            //Loader loader = JsonConvert.DeserializeObject<Loader>(textAsset.text);
            Loader loader = JsonUtility.FromJson<Loader>(textAsset.text);
            callback?.Invoke(loader);
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
