using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIController_Base : MonoBehaviour
{
    protected PlayerInput _inputActions;
    private void Awake()
    {
        Init();
       
    }
    private void OnEnable()
    {
        EnableInput();
    }
    private void OnDisable()
    {
        DisableInput();
    }
    protected virtual bool Init()
    {
        _inputActions = new PlayerInput();
        if (_inputActions != null) return true;

        return false;
    }
    protected abstract  void EnableInput();
    protected abstract void DisableInput();

}
