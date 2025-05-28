using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


[Serializable]
public class InteractEntry
{
    public Define.InteractType Type;
    public int Count;
}

[Serializable]
public class InteractRecordData
{
    public List<InteractEntry> Entries = new();

    public void Add(Define.InteractType type)
    {
        var entry = Entries.Find(e => e.Type == type);
        if (entry == null)
        {
            entry = new InteractEntry { Type = type, Count = 0 };
            Entries.Add(entry);
        }
        entry.Count++;
    }

    public int Get(Define.InteractType type)
    {
        var entry = Entries.Find(e => e.Type == type);
        return entry?.Count ?? 0;
    }
    public int GetAllCount()
    {
        int count = 0;
        for (int i = 0; i < Entries.Count; i++)
        {
            count += Entries[i].Count;
        }
        return count;
    }
}