using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/InteractTutorialData")]
public class InteractTutorialMapSO : ScriptableObject
{
    public List<InteractTutorialEntry> Entries = new();

    [Serializable]
    public class InteractTutorialEntry
    {
        public InteractType Type;
        public string PopupPrefabName;
    }

    public string GetPopupName(InteractType type)
    {
        var entry = Entries.Find(e=>e.Type==type);
        return entry?.PopupPrefabName;
    }
}
