using UnityEngine;

public abstract class SpawnBase : MonoBehaviour
{
    public TextAsset spawnerDataText;

    protected SpawnerData spawnerData;

    protected virtual bool LoadSpawnerData()
    {
        try
        {
            spawnerData = JsonUtility.FromJson<SpawnerData>(spawnerDataText.text);
        }
        catch (System.Exception)
        {           
            return false;
        }

        return true;
    }

    protected abstract void SpawnObj();

    protected virtual void Start()
    {
        if (!LoadSpawnerData())
        {
            enabled = false;
            return;
        }

        SpawnObj();
    }
}
