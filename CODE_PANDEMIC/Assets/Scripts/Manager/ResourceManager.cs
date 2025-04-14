using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
public class ResourceManager : MonoBehaviour
{

    Dictionary<string, Object> _cache = new Dictionary<string, Object>();
    Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();

    int _count = 0;
    public int Count { get { return _count; } }
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        if (_cache.TryGetValue(key, out Object resource))
        {
            callback?.Invoke(resource as T);
            return;
        }
        if (_handles.ContainsKey(key))
        {
            _handles[key].Completed += (op) => {
                callback?.Invoke(op.Result as T);
            };
            return;
        }
        _handles.Add(key, Addressables.LoadAssetAsync<T>(key));
        _count++;
        _handles[key].Completed += (op) =>
        {
            _cache.Add(key, op.Result as UnityEngine.Object);
            callback?.Invoke(op.Result as T);
            _count--;
        };

    }
    public void Remove(string key)
    {
        if (_cache.TryGetValue(key, out Object resource) == false) return;
        _cache.Remove(key);
        if (_handles.TryGetValue(key, out AsyncOperationHandle handle) == false)
        {
            Addressables.Release(handle);
        }
        _handles.Remove(key);
    }
    public void Clear()
    {
        _cache.Clear();
        foreach (var handle in _handles.Values)
        {
            Addressables.Release(handle);
        }
        _handles.Clear();
    }

    //addressable name , parent, callback
    public void Instantiate(string key, Transform parent = null, Action<GameObject> callback = null)
    {
        LoadAsync<GameObject>(key, (prefab) =>
        {
            GameObject obj = Instantiate(prefab, parent);
            obj.name = prefab.name;
            obj.transform.localPosition = prefab.transform.position;
            callback?.Invoke(obj);
        });
    }

    
    public void Destroy(GameObject obj, float seconds = 0.0f)
    {
        Object.Destroy(obj, seconds);

    }


}
