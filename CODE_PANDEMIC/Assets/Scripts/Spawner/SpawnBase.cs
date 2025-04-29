using UnityEngine;

public abstract class SpawnBase : MonoBehaviour
{
   
    protected virtual void OnDestroy()
    {
        UnRegister();
    }
    public abstract void SpawnObjects();
    protected void Register()
    {
        Managers.Object.RegisterSpawners();
    }
    protected  void UnRegister()
    {
        Managers.Object.UnregisterSpawners();
    }
}
