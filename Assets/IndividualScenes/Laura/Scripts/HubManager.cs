using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    public static HubManager Instance { get; private set;}
    

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Animator _animationController;

    //Input
    private InputActionMap _hub;
    private InputAction _move;
    private InputAction _submit;
    private InputAction _cancel;

    private bool _isOnPlatform;

    //Rotation
    [SerializeField] private float _levelZOffset = 4f;
    [SerializeField] private float _rotationSpeed;
    private bool _isLookingFront = true;
    private Quaternion _targetRotation;
    private float _rotationCloseFactor = 0.9999f;

    //Load selection
    [SerializeField] private AudioClip _levelSelectionAudio;
    [SerializeField] private AudioClip _levelSelectionIncorrectAudio;

    [SerializeField] private LockedLevelController _lockedLevelController;
    [SerializeField] private UnlockedLevelController _unlockedLevelController;

    [SerializeField] private float _yOffset;
    private bool _inMotion;

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

    private void Start()
    {
        if (SaveGameManager.Instance.IsDataSaved())
        {
            //Load from save file
            SaveGameManager.Instance.LoadSessionDataFromFile();
        }
        transform.position = SaveGameManager.Instance.SessionData.SessionLevels[SaveGameManager.Instance.SessionData.CurrentLevelIndex].Position;
    }

    private void Update()
    {
        if (!_isOnPlatform)
        {
            if (DestinationReached())
            {
                MarkAsPlatformArrival();
                SelectRotationTarget();
                SessionLevel level = SaveGameManager.Instance.SessionData.SessionLevels[SaveGameManager.Instance.SessionData.CurrentLevelIndex];
                if (level.isPlayable)
                {
                    if (level.IsLocked)
                    {
                        ShowNumberOfCarrotsToUnlock(level);
                    }
                    else
                    {
                        ShowCurrentCarrotsInLevel(level);
                    }
                }
            }
            
        }
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
        return levelIndex >= 0 && levelIndex < SaveGameManager.Instance.SessionData.SessionLevels.Count;
    }

    public void MoveToLevel(int levelIndex)
    {
       _agent.SetDestination(SaveGameManager.Instance.SessionData.SessionLevels[SaveGameManager.Instance.SessionData.CurrentLevelIndex].Position);
        MarkAsPlatformDeparture();
       _animationController.Play("Walking");
       _isLookingFront = false;
    }

    public void Navigate(InputAction.CallbackContext ctx)
    {
       Vector2 move = ctx.ReadValue<Vector2>();
        int tmpIndex;
       if (move == Vector2.left)
       {
            tmpIndex = SaveGameManager.Instance.SessionData.CurrentLevelIndex - 1;
            if (IsLevelReachable(tmpIndex))
            {
                SaveGameManager.Instance.SessionData.CurrentLevelIndex = tmpIndex;
                _agent.isStopped = false;
                MoveToLevel(tmpIndex);
            }
       }
       else if (move == Vector2.right)
       {
            tmpIndex = SaveGameManager.Instance.SessionData.CurrentLevelIndex + 1;
            if (IsLevelReachable(tmpIndex))
            {
                SaveGameManager.Instance.SessionData.CurrentLevelIndex = tmpIndex;
                _agent.isStopped = false;
                MoveToLevel(tmpIndex);
            }
        }
    }

    private void SelectRotationTarget()
    {
        Vector3 lookAtDirection = SaveGameManager.Instance.SessionData.SessionLevels[SaveGameManager.Instance.SessionData.CurrentLevelIndex].Position;
        lookAtDirection.z = lookAtDirection.z + _levelZOffset;
        Vector3 direction = ( lookAtDirection - transform.position).normalized;
        _targetRotation = Quaternion.LookRotation(direction);
    }

    public void Submit(InputAction.CallbackContext ctx)
    {
        SessionLevel currentLevel = SaveGameManager.Instance.SessionData.SessionLevels[SaveGameManager.Instance.SessionData.CurrentLevelIndex];
        if(_isOnPlatform)
        {
           if(currentLevel.isPlayable)
            {
                if (!currentLevel.IsLocked)
                {
                    Debug.Log("Selecting level");
                    StartCoroutine(LoadLevel(currentLevel));
                }
                else
                {
                    //Check if number of carrots is enough to unlock the level
                    if(CanBeUnlocked(currentLevel))
                    {
                        //We can unlock the level.
                        StartCoroutine(UnlockLevel(currentLevel));
                    }
                    else
                    {
                        SelectLockedLevel();
                    }
                }
            }
        }
    }

    private bool CanBeUnlocked(SessionLevel currentLevel)
    {
        return currentLevel.NumberOfCarrotsToUnlock <= SaveGameManager.Instance.SessionData.TotalNumberOfCarrots;
    }

    private IEnumerator UnlockLevel(SessionLevel level)
    {
        yield return new WaitForSeconds(1);
        //Do a lot of things
        Unlock(level);
    }

    private void Unlock(SessionLevel level)
    {
        HideNumberOfCarrotsToUnlock();
        ShowCurrentCarrotsInLevel(level);
        level.IsLocked = false;
    }

    public Vector3 GetCanvasPosition(SessionLevel level)
    {
        return new Vector3(level.Position.x, level.Position.y + _yOffset, level.Position.z);
    }

    public int GetNumberOfCarrotsPicked(SessionLevel level)
    {
        return level.SessionCarrots.Where(carrot => carrot.IsPicked).Count();
    }

    private IEnumerator LoadLevel(SessionLevel level)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(level.SceneName);
        /*
        _animationController.Play("IdleB");
        _particleSystemSelect.transform.position = level.transform.position + new Vector3 (0, level._yOffsetTop, 0);
        _particleSystemSelect.gameObject.SetActive(true);
        _particleSystemSelect.Play();
        yield return new WaitForSeconds(level.LoadLevelDelay);
        //AudioManager.Instance.PlaySFX(_selectionAudio);
        SceneManager.LoadScene(level.SceneName);*/
    }

    private void SelectLockedLevel()
    {
        //AudioManager.Instance.PlaySFX(_selectionIncorrectAudio);
    }

    public void Cancel(InputAction.CallbackContext ctx)
    {
        //Open Pause Menu on HUB, disable 
        UIManager.Instance.EnableHUBPauseMenu();
        //Disable controls on hub
        _playerInput.uiInputModule.ActivateModule();
        _hub.Disable();
    }

    public void ReturnControlsToPlayer()
    {
        _playerInput.uiInputModule.DeactivateModule();
        _hub.Enable();
    }

    public void MarkAsPlatformArrival()
    {
        _isOnPlatform = true;
        _agent.isStopped = true;
        _animationController.Play("IdleA");
    }

    public void MarkAsPlatformDeparture()
    {
        _isOnPlatform = false;
        _inMotion = true;
        HideNumberOfCarrotsToUnlock();
        HideCurrentCarrotsInLevel();
    }

    public void ShowNumberOfCarrotsToUnlock(SessionLevel level)
    {
        _lockedLevelController.ShowNumberOfCarrotsToUnlock(level.NumberOfCarrotsToUnlock, GetCanvasPosition(level));
    }

    public void ShowCurrentCarrotsInLevel(SessionLevel level)
    {
        _unlockedLevelController.ShowCurrentCarrotsInLevel(GetNumberOfCarrotsPicked(level), GetCanvasPosition(level));
    }

    public void HideNumberOfCarrotsToUnlock()
    {
        _lockedLevelController.HideNumberOfCarrotsToUnlock();
    }

    public void HideCurrentCarrotsInLevel()
    {
        _unlockedLevelController.HideCurrentCarrotsInLevel();
    }

    public bool DestinationReached()
    {
        return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
    }

    /*public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Level") && DestinationReached())
        {
            //MarkAsPlatformArrival();
            SelectRotationTarget();
            SessionLevel level = SaveGameManager.Instance.SessionData.SessionLevels[SaveGameManager.Instance.SessionData.CurrentLevelIndex];
            if (level.isPlayable)
            {
                if(level.IsLocked)
                {
                    ShowNumberOfCarrotsToUnlock(level);
                }
                else
                {
                    ShowCurrentCarrotsInLevel(level);
                }
            }
        }
    }*/

    /*public void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Level") )
        {
            MarkAsPlatformDeparture();
            SessionLevel level = SaveGameManager.Instance.SessionData.SessionLevels[SaveGameManager.Instance.SessionData.CurrentLevelIndex];
            if (level.isPlayable)
            {
                if (level.IsLocked)
                {
                    HideNumberOfCarrotsToUnlock();
                }
                else
                {
                    HideCurrentCarrotsInLevel();
                }
            }

        }
    }*/
}
