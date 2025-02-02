using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class ControllerChange : MonoBehaviour
{
    private PlayerInput _playerInput;

    [SerializeField] private string keyboardControlScheme = "Keyboard&Mouse";
    [SerializeField] private string gamepadControlScheme = "Gamepad";

    public void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.onControlsChanged += OnControlsChanged;
    }


    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme == keyboardControlScheme)
        {
            UIManager.Instance.ChangeToKeyboard();
        }
        else if (input.currentControlScheme == gamepadControlScheme)
        {
            UIManager.Instance.ChangeToGamepad();
        }
    }

}
