using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private playerMovement _playerMovement;
    [SerializeField] private playerJump _playerJump;

    private InputActionMap _player;
    private InputAction _move;
    private InputAction _jump;
    private InputAction _quit;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        _player = _playerInput.actions.FindActionMap("Player");
        _move = _player.FindAction("Move");
        _jump = _player.FindAction("Jump");
        _quit = _player.FindAction("Quit");
    }

    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    public void EnableInput()
    {
        /*_player.Enable();
        _move.performed += _playerMovement.OnMovement;
        _jump.performed += _playerJump.OnJump;
        _jump.performed += hudManager.instance.refillBar;
        _quit.performed += Cancel;*/
    }

    public void DisableInput()
    {
        /*_player.Disable();
        _move.performed -= _playerMovement.OnMovement;
        _jump.performed -= _playerJump.OnJump;
        _jump.performed -= hudManager.instance.refillBar;
        _quit.performed -= Cancel;*/
    }

    public void Cancel(InputAction.CallbackContext ctx)
    {
        UIManager.Instance.EnableLevelPauseMenu();
        //Time.timeScale = 0;
        DisableInput();
        _playerInput.uiInputModule.ActivateModule();
    }

    public void ReturnControlsToPlayer()
    {
        _playerInput.uiInputModule.DeactivateModule();
        //Time.timeScale = 1;
        EnableInput();
    }

}
