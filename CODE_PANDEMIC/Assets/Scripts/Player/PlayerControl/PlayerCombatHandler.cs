using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerCombatHandler : MonoBehaviour
{
    private PlayerInputHandler _inputHandler;
    private EquipWeapon _equipWeapon;
    private PlayerController _playerController;

    public void Initialize(PlayerController controller)
    {
        _playerController = controller;
        _inputHandler = GetComponent<PlayerInputHandler>();
        _equipWeapon = GetComponent<EquipWeapon>();
    }

    public void SubscribeInput()
    {
        _inputHandler.ReloadAction.performed += PerformReload;
    }

    public void UnsubscribeInput()
    {
        _inputHandler.ReloadAction.performed -= PerformReload;
    }

    public void HandleAttackInput()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _equipWeapon?.StartAttack(_playerController);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _equipWeapon?.StopAttack();
        }
    }

    public void StopAttack()
    {
        _equipWeapon?.StopAttack();
    }

    private void PerformReload(InputAction.CallbackContext context)
    {
        _equipWeapon?.Reload();
    }
}