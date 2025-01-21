using System.Collections;
using System.Collections.Generic;
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

    //Level info
    [SerializeField] private Level[] _levels;
    private int _currentLevelIndex = 0;
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
        /*if (!SaveGameManager.CurrentSessionData)
        {
            if (SaveGameManager.Instance.IsDataSaved())
            {
                SaveGameManager.CurrentSessionData = ScriptableObject.CreateInstance<DataBetweenScenes>();
                SaveGameManager.Instance.LoadJsonData(SaveGameManager.CurrentSessionData);
            }
            else
            {
                SaveGameManager.Instance.InitializeDataFromScene();
            }
        }
        else
        {
            Debug.Log(SaveGameManager.CurrentSessionData);
        }
        transform.position = SaveGameManager.CurrentSessionData.LevelInfo[SaveGameManager.Instance.CurrentLevelIndex].transform.position;
        */

        if (SceneManager.GetActiveScene().name == "HUB")
        {
            //SaveGameManager.Instance.SessionData.CurrentLevelIndex = 1;
            //SaveGameManager.Instance.SessionData.LevelInfo = new List<SaveData.LevelData>();
            SaveData.LevelData level = new SaveData.LevelData();
            level.IsLocked = true;
            SaveGameManager.Instance.SessionData.LevelInfo.Add(level);

            /*string json = JsonUtility.ToJson(SessionData);
            Debug.Log(json);*/

            //SaveGameManager.Instance.AnotherSessionData = ScriptableObject.CreateInstance<SessionData>();
            //JsonUtility.FromJsonOverwrite(json, SaveGameManager.Instance.AnotherSessionData);

            //Debug.Log(SaveGameManager.Instance.AnotherSessionData.LevelInfo[0].IsLocked);

            //if (SaveGameManager.Instance.SessionData.LevelInfo[0].CarrotList != null)
            //{
            //Debug.Log($"Picked: {SaveGameManager.Instance.SessionData.LevelInfo[0].CarrotList[0].IsPicked}");
            //}
            string json = JsonUtility.ToJson(SaveGameManager.Instance.SessionData);
            Debug.Log(json);

            SceneManager.LoadScene("Level0");
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
        return levelIndex >= 0 && levelIndex < SaveGameManager.CurrentSessionData.LevelInfo.Count;
    }

    public void MoveToLevel(int levelIndex)
    {
       _agent.SetDestination(SaveGameManager.CurrentSessionData.LevelInfo[SaveGameManager.Instance.CurrentLevelIndex].transform.position);
       _animationController.Play("Walking");
       _isLookingFront = false;
    }

    public void Navigate(InputAction.CallbackContext ctx)
    {
       Vector2 move = ctx.ReadValue<Vector2>();
        int tmpIndex;
       if (move == Vector2.left)
       {
            tmpIndex = SaveGameManager.Instance.CurrentLevelIndex - 1;
            if (IsLevelReachable(tmpIndex))
            {
                SaveGameManager.Instance.CurrentLevelIndex = tmpIndex;
                _agent.isStopped = false;
                MoveToLevel(tmpIndex);
            }
       }
       else if (move == Vector2.right)
       {
            tmpIndex = SaveGameManager.Instance.CurrentLevelIndex + 1;
            if (IsLevelReachable(tmpIndex))
            {
                SaveGameManager.Instance.CurrentLevelIndex = tmpIndex;
                _agent.isStopped = false;
                MoveToLevel(tmpIndex);
            }
        }
    }

    private void SelectRotationTarget()
    {
        Vector3 lookAtDirection = SaveGameManager.CurrentSessionData.LevelInfo[SaveGameManager.Instance.CurrentLevelIndex].transform.position;
        lookAtDirection.z = lookAtDirection.z + _levelZOffset;
        Vector3 direction = ( lookAtDirection - transform.position).normalized;
        _targetRotation = Quaternion.LookRotation(direction);
    }

    public void Submit(InputAction.CallbackContext ctx)
    {
        Level currentLevel = SaveGameManager.CurrentSessionData.LevelInfo[SaveGameManager.Instance.CurrentLevelIndex];
        if(_isOnPlatform)
        {
           PlayableLevel level = currentLevel as PlayableLevel;
           if(level != null)
            {
                if (!level.IsLocked)
                {
                    Debug.Log("Selecting level");
                    StartCoroutine(LoadLevel(level));
                }
                else
                {
                    //Check if number of carrots is enough to unlock the level
                    if(level.CanBeUnlocked())
                    {
                        //We can unlock the level.
                        StartCoroutine(UnlockLevel(level));
                    }
                    SelectLockedLevel();
                }
            }
        }
    }

    private IEnumerator UnlockLevel(PlayableLevel level)
    {
        yield return new WaitForSeconds(1);
        //Do a lot of things
        level.UnLock();

    }

    private IEnumerator LoadLevel(PlayableLevel level)
    {


        yield return new WaitForSeconds(5f);
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


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Level"))
        {
            SelectRotationTarget();
        }
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
    }

    public void ShowNumberOfCarrotsToUnlock(int carrotsToUnlock, Vector3 canvasPosition)
    {
        _lockedLevelController.ShowNumberOfCarrotsToUnlock(carrotsToUnlock, canvasPosition);
    }

    public void ShowCurrentCarrotsInLevel(int carrotsUnlocked, Vector3 canvasPosition)
    {
        _unlockedLevelController.ShowCurrentCarrotsInLevel(carrotsUnlocked, canvasPosition);
    }

    public void HideNumberOfCarrotsToUnlock()
    {
        _lockedLevelController.HideNumberOfCarrotsToUnlock();
    }

    public void HideCurrentCarrotsInLevel()
    {
        _unlockedLevelController.HideCurrentCarrotsInLevel();
    }

}
