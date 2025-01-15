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
    private int _currentLevelIndex;
    private bool _isOnPlatform;

    private InputActionMap _hub;
    private InputAction _move;
    private InputAction _submit;
    private InputAction _cancel;
    private bool _isLookingFront = true;

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
            FaceTarget();
            Debug.Log("Rotation: " + transform.rotation);
        }
    }

    void FaceTarget()
    {
        Vector3 lookAtDirection = transform.position;
        lookAtDirection.z = lookAtDirection.z - 1;
        Vector3 direction = (lookAtDirection - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.z, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);

        if (Quaternion.Dot(transform.rotation, lookRotation) > 0.8f) 
        {
            _isLookingFront = true;
            Debug.Log("Looking front");
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
                Debug.Log("Reachable:" + tmpIndex);
                _currentLevelIndex = tmpIndex;
                _agent.isStopped = false;
                MoveToLevel(_currentLevelIndex);
            }
            else
            {
                Debug.Log("Left level not available");
            }

       }
       else if (move == Vector2.right)
       {
            tmpIndex = _currentLevelIndex + 1;
            if (IsLevelReachable(tmpIndex))
            {
                Debug.Log("Reachable:" + tmpIndex);
                _currentLevelIndex = tmpIndex;
                _agent.isStopped = false;
                MoveToLevel(_currentLevelIndex);
            }
            else
            {
                Debug.Log("Right level not available");
            }
        }
        
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
