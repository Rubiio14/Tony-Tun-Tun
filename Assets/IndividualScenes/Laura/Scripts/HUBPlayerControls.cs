using System;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(NavMeshAgent))]
public class HUBPlayerControls : MonoBehaviour
{
    [SerializeField]private NavMeshAgent _agent;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Animator _animationController;
    [SerializeField] private Level[] _levels;
    private int _currentLevelIndex = 0;
    private bool _isOnPlatform;

    private InputActionMap _hub;
    private InputAction _move;
    private InputAction _submit;
    private InputAction _cancel;
    private bool _isLookingFront = true;

    [SerializeField] private float _levelZOffset = 4f;
    [SerializeField] private float _rotationSpeed;
    private Quaternion _targetRotation;
    private float _rotationCloseFactor = 0.9999f;

    public void Awake()
    {
        //_agent = GetComponent<NavMeshAgent>();
        //_playerInput = GetComponent<PlayerInput>();
        _hub = _playerInput.actions.FindActionMap("HUB");
        _move = _hub.FindAction("Navigate");
        _submit = _hub.FindAction("Submit");
        _cancel = _hub.FindAction("Cancel");
        _move.performed += Navigate;
        _submit.performed += Submit;
        _cancel.performed += Cancel;
    }

    void OnEnable()
    {
        _move.Enable();
        _submit.Enable();
        _cancel.Enable();
        _hub.Enable();
    }

    void OnDisable()
    {
        _move.Disable();
        _submit.Disable();
        _cancel.Disable();
        _hub.Disable();
    }

    private void Update()
    {
        if (_isOnPlatform && !_isLookingFront)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            if(Quaternion.Dot(transform.rotation, _targetRotation) >= _rotationCloseFactor)
            {
                _isLookingFront = true;
            }
        }
    }

    public bool IsLevelReachable(int levelIndex)
    {
        return levelIndex >= 0 && levelIndex < _levels.Length && _levels[levelIndex].IsTransitable;
    }

    public void MoveToLevel(int levelIndex)
    {
       _agent.SetDestination(_levels[_currentLevelIndex].Location);
       _animationController.Play("Walking");
       _isLookingFront = false;
    }

    public void Navigate(InputAction.CallbackContext ctx)
    {
       Vector2 move = ctx.ReadValue<Vector2>();
        int tmpIndex = 0;
       if (move == Vector2.left)
       {
            tmpIndex = _currentLevelIndex - 1;
            if (IsLevelReachable(tmpIndex))
            {
                _currentLevelIndex = tmpIndex;
                _agent.isStopped = false;
                MoveToLevel(_currentLevelIndex);
            }
            else
            {
                
            }

       }
       else if (move == Vector2.right)
       {
            tmpIndex = _currentLevelIndex + 1;
            if (IsLevelReachable(tmpIndex))
            {
                _currentLevelIndex = tmpIndex;
                _agent.isStopped = false;
                MoveToLevel(_currentLevelIndex);
            }
            else
            {
                
            }
        }
        
    }

    private void SelectRotationTarget()
    {
        Vector3 lookAtDirection = _levels[_currentLevelIndex].transform.position;
        lookAtDirection.z = lookAtDirection.z + _levelZOffset;

        Vector3 direction = ( lookAtDirection - transform.position).normalized;
        _targetRotation = Quaternion.LookRotation(direction);

    }

    public void Submit(InputAction.CallbackContext ctx)
    {
        Level currentLevel = _levels[_currentLevelIndex];
        if (currentLevel.IsAccessible && _isOnPlatform)
        {
            Debug.Log("Selecting level");
            _animationController.Play("LevelSelect");
            currentLevel.SelectLevel();
        }
    }

    public void Cancel(InputAction.CallbackContext ctx)
    {
        //Open Pause Menu on HUB, disable 
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Level"))
        {
            _isOnPlatform = true;
            _agent.isStopped = true;
            _animationController.Play("IdleA");
            SelectRotationTarget();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Level"))
        {
            _isOnPlatform = false;
        }
    }
}
