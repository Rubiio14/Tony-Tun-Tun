using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    public static HubManager Instance { get; private set;}
    
    //Components
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Animator _animationController;

    //Input
    private InputActionMap _player;
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
    [SerializeField] private float _unlockLevelDelay;
    [SerializeField] private float _loadLevelDelay;
    [SerializeField] private AudioClip _levelSelectionAudio;
    [SerializeField] private AudioClip _levelSelectionIncorrectAudio;

    [SerializeField] private float _yOffset;
    [SerializeField] private LockedLevelController _lockedLevelController;
    [SerializeField] private UnlockedLevelController _unlockedLevelController;

    [SerializeField] private ParticleSystem _particleSystemUnlock;

    [SerializeField] private GameObject _enemyCinematic;
    [SerializeField] private float _enemyWalkingDuration;

    private Dictionary<int, LevelModelSwitcher> _switchers = new Dictionary<int, LevelModelSwitcher>();
    

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
        _submit = _player.FindAction("Jump");
        _cancel = _player.FindAction("Quit");
    }

    void OnEnable()
    {
        EnableInput();
    }

    void OnDisable()
    {
        DisableInput();
    }

    public void EnableInput()
    {
        _player.Enable();
        _move.performed += Move;
        _submit.performed += Submit;
        _cancel.performed += Cancel;
    }
    
    public IEnumerator EnableAfterDelay()
    {
        yield return new WaitForSeconds(_enemyWalkingDuration);
        EnableInput();
    }
    
    public void DisableInput()
    {
        _player.Disable();
        _move.performed -= Move;
        _submit.performed -= Submit;
        _cancel.performed -= Cancel;
    }

    private void Start()
    {
        //Session not yet started and there is data in file
        if (!SaveGameManager.IsSessionStarted)
        {
            if (SaveGameManager.Instance.IsDataSavedInFile())
            {
                //Load from save file
                SaveGameManager.Instance.LoadSessionDataFromFile();
                SaveGameManager.IsSessionStarted = true;
            }
            else
            {
                //First time in hub and there was no saved data
                _enemyCinematic.SetActive(true);
                DisableInput();
                StartCoroutine(EnableAfterDelay());
            }
        }
        transform.position = SaveGameManager.Instance.SessionData.SessionLevels[SaveGameManager.Instance.SessionData.CurrentLevelIndex].Position;
        _isOnPlatform = true;
        _isLookingFront = true;
        SaveGameManager.IsSessionStarted = true;
        LoadLevelRepresentation();
        FMODAudioManager.instance.InitializeMusic(FMODEvents.instance.levelMusic);
    }

    private void LoadLevelRepresentation()
    {
        LevelModelSwitcher[] switchersInHUB = GameObject.FindObjectsByType<LevelModelSwitcher>(FindObjectsSortMode.None);
        for(int i = 0; i < switchersInHUB.Length; i++)
        {
            _switchers.Add(switchersInHUB[i].Identifier, switchersInHUB[i]);
            switchersInHUB[i].SwitchRepresentation(SaveGameManager.Instance.SessionData.SessionLevels[switchersInHUB[i].Identifier].IsLocked);
        }
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
        return levelIndex >= 0 && levelIndex < SaveGameManager.Instance.SessionData.SessionLevels.Count;
    }

    public void MoveToLevel(int levelIndex)
    {
       _agent.SetDestination(SaveGameManager.Instance.SessionData.SessionLevels[SaveGameManager.Instance.SessionData.CurrentLevelIndex].Position);
        MarkAsPlatformDeparture();
       _animationController.Play("Walking");
       _isLookingFront = false;
    }

    public void Move(InputAction.CallbackContext ctx)
    {
       if (DestinationReached())
       {
            float move = ctx.ReadValue<float>();
            int tmpIndex;
            if (move == Vector2.left.x)
            {
                tmpIndex = SaveGameManager.Instance.SessionData.CurrentLevelIndex - 1;
                if (IsLevelReachable(tmpIndex))
                {
                    SaveGameManager.Instance.SessionData.CurrentLevelIndex = tmpIndex;
                    _agent.isStopped = false;
                    MoveToLevel(tmpIndex);
                }
            }
            else if (move == Vector2.right.x)
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
        if (DestinationReached())
        {
            SessionLevel currentLevel = SaveGameManager.Instance.SessionData.SessionLevels[SaveGameManager.Instance.SessionData.CurrentLevelIndex];
            if(_isOnPlatform)
            {
                if(currentLevel.isPlayable)
                {
                    if (!currentLevel.IsLocked)
                    {
                        FMODAudioManager.instance.StopMusic();
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
    }

    private bool CanBeUnlocked(SessionLevel currentLevel)
    {
        return currentLevel.NumberOfCarrotsToUnlock <= SaveGameManager.Instance.GetTotalPickedCarrots();
    }

    private IEnumerator UnlockLevel(SessionLevel level)
    {
        //TODO: Fix with correct audio sound
        //FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.rayoCollectedSound, level.Position);
        _particleSystemUnlock.transform.position = level.Position;
        _particleSystemUnlock.gameObject.SetActive(true);
        _particleSystemUnlock.Play();

        yield return new WaitForSeconds(_unlockLevelDelay);

        Unlock(level);

        yield return new WaitForSeconds(_unlockLevelDelay);

        _particleSystemUnlock.gameObject.SetActive(false);
    }

    private void Unlock(SessionLevel level)
    {
        HideNumberOfCarrotsToUnlock();
        ShowCurrentCarrotsInLevel(level);
        level.IsLocked = false;
        _switchers[level.Identifier].SwitchRepresentation(level.IsLocked);
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
        _animationController.Play("IdleB");
        DisableInput();
        yield return new WaitForSeconds(_loadLevelDelay);
        SceneManager.LoadScene(level.SceneName);
    }

    private void SelectLockedLevel()
    {
        //AudioManager.Instance.PlaySFX(_selectionIncorrectAudio);
    }

    public void Cancel(InputAction.CallbackContext ctx)
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.openPause);
        UIManager.Instance.EnableHUBPauseMenu();
        _playerInput.uiInputModule.ActivateModule();
        DisableInput();
    }

    public void ReturnControlsToPlayer()
    {
        _playerInput.uiInputModule.DeactivateModule();
        EnableInput();
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
        return _agent && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Level"))
        {
            MarkAsPlatformArrival();
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
    }

    public void OnTriggerExit(Collider collision)
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
    }
}
