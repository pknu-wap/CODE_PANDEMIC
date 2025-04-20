using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempDamage : MonoBehaviour
{
    [SerializeField] PlayerStatus _status;
    PlayerInput _inputActions;
    public PlayerStatus Status { get { return _status; } set { _status = value; } }
    void  Awake()
    {
        _inputActions=new PlayerInput();
    }
    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Damage.performed -= Damaged;

        _inputActions.Player.Damage.performed += Damaged;
    }
    private void OnDisable()
    {
        _inputActions.Player.Damage.performed -= Damaged;
    }

    private void Damaged(InputAction.CallbackContext context)
    {
    
        _status.OnDamaged(null, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
