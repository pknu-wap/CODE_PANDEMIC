using System;
using System.Collections.Generic;

public enum InteractType
{
    Generator,
    Extinguisher,
    WaterPump,
    // Add more as needed
}

[Serializable]
public class InteractRecordData
{
    public Dictionary<InteractType, int> InteractCounts = new();

    public void Add(InteractType type)
    {
        if (!InteractCounts.ContainsKey(type))
            InteractCounts[type] = 0;

        InteractCounts[type]++;
    }

    public int Get(InteractType type)
    {
        return InteractCounts.TryGetValue(type, out int value) ? value : 0;
    }
}
