using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    Dictionary<string, Action<object>> eventDictionary = new Dictionary<string, Action<object>>();

    public void Subscribe(string eventName, Action<object> listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] = delegate { };

        eventDictionary[eventName] += listener;
    }
    public void Unsubscribe(string eventName, Action<object> listener)
    {
        if (!eventDictionary.ContainsKey(eventName)) return;
        eventDictionary[eventName] -= listener;
    }
    public void InvokeEvent(string eventName, object parameter = null)
    {
        if (!eventDictionary.ContainsKey(eventName)) return;
        eventDictionary[eventName]?.Invoke(parameter);
    }
}
