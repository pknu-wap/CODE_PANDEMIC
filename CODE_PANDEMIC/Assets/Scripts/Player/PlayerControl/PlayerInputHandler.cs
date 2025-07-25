using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _runAction;
    private InputAction _dashAction;

    public void EnableInput()
    {
        _playerInput = new PlayerInput();
        _moveAction = _playerInput.Player.Move;
        _runAction = _playerInput.Player.Run;
        _dashAction = _playerInput.Player.Dash;

        _playerInput.Enable();
        _moveAction.Enable();
        _runAction.Enable();
        _dashAction.Enable();
    }

    public void DisableInput()
    {
        _playerInput.Disable();
    }

    public Vector2 GetMoveInput() => _moveAction.ReadValue<Vector2>();
    public bool IsRunPressed() => _runAction.IsPressed();
    public bool IsDashTriggered() => _dashAction.triggered;
    public InputAction ReloadAction => _playerInput.Player.Reload;
}
