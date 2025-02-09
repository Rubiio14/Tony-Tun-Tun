using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //Menus
    [SerializeField] private MainMenuUIController _mainMenu;
    [SerializeField] private OptionsUIController _optionsMenu;
    [SerializeField] private CreditsUIController _creditsMenu;
    [SerializeField] private PauseUIController _pauseMenu;

    //Confirmation
    [SerializeField] private ConfirmationController _confirmation;
   
    //Localization
    public static Locale[] AvailableLocales { get; private set; }
    public StringTable UITextTable { get; private set; }

    //Resolution
    public static Resolution[] AvailableResolutions { get; private set; }

    //Controller
    //TODO: Add removal of listeners at some point
    public event Action<bool> OnControllerChange;

    //Global variables for UI localization
    private readonly string UITableName = "UI";
    private readonly string KeyboardKey = "Keyboard";
    private readonly string GamepadKey = "Gamepad";

    //Icon change
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private string _keyboardControlScheme = "Keyboard&Mouse";
    [SerializeField] private string _gamepadControlScheme = "Gamepad";

    //Scene change
    [Header("Delay will be added after fade duration")]
    [SerializeField] private float _delayForSceneChange;
    [SerializeField] private CanvasGroup _loadingScreen;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _fadeIncrement;

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
    }

    public void Start()
    {
        if (!SaveGameManager.IsSessionStarted)
        {
            if (SaveGameManager.Instance.IsDataSavedInFile())
            {
                //Load from save file
                SaveGameManager.Instance.LoadSessionDataFromFile();
            }
            SaveGameManager.IsSessionStarted = true;
            AvailableResolutions = Screen.resolutions.Where(res => res.refreshRateRatio.value >= 60f).ToArray();
            AvailableLocales = LocalizationSettings.AvailableLocales.Locales.ToArray();
        }

        //Resolution initialization
        LoadResolution();

        //Localization initialization
        LoadLocale();

        //Volume and music initialization
        FMODAudioManager.instance.LoadVolumes();

        UITextTable = LocalizationSettings.StringDatabase.GetTable(UITableName);

        _playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDestroy()
    {
        _playerInput.onControlsChanged -= OnControlsChanged;
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme == _keyboardControlScheme)
        {
            ChangeToKeyboard();
        }
        else if (input.currentControlScheme == _gamepadControlScheme)
        {
            ChangeToGamepad();
        }
    }

    private void LoadLocale()
    {
        LocalizationSettings.SelectedLocale = AvailableLocales.First(locale => locale.Identifier.Code == SaveGameManager.Instance.SessionData.Locale);
    }

    private void LoadResolution()
    {
        Screen.SetResolution(SaveGameManager.Instance.SessionData.SessionResolution.Width, SaveGameManager.Instance.SessionData.SessionResolution.Height, FullScreenMode.FullScreenWindow);
    }

    public void UpdateLanguage()
    {
        UITextTable = LocalizationSettings.StringDatabase.GetTable(UITableName);
    }

    public String GetLocalizedUIText(String localizedKey)
    {
        return UITextTable.GetEntry(localizedKey).LocalizedValue;
    }

    public IEnumerator LoadScene(string sceneName)
    {
        //meter fundido a negro
        StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(_fadeDuration + _delayForSceneChange);
        SceneManager.LoadScene(sceneName);
    }

    public void EnableMainMenu()
    {
        _mainMenu.gameObject.SetActive(true);
    }

    public void EnableOptionsMenu()
    {
        _optionsMenu.gameObject.SetActive(true);
        _optionsMenu.SelectFirst();
    }

    public void DisableOptionsMenu()
    {
        _optionsMenu.gameObject.SetActive(false);
    }

    public void EnableCreditsMenu()
    {
        _creditsMenu.gameObject.SetActive(true);
    }
    public void DisableCreditsMenu()
    {
        _creditsMenu.gameObject.SetActive(false);
    }
    public void EnableHUBPauseMenu()
    {
        _pauseMenu.gameObject.SetActive(true);
    }

    public void DisableHUBPauseMenu()
    {
        HubManager.Instance.ReturnControlsToPlayer();
    }

    public void EnableLevelPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.openPause);
            _playerInput.SwitchCurrentActionMap("UI");
            _pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
            _pauseMenu.SelectFirstElement();
        }
    }

    public void DisableLevelPauseMenu()
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.closePause);
        _playerInput.SwitchCurrentActionMap("Player");
        Time.timeScale = 1;
        _pauseMenu.gameObject.SetActive(false);
    }

    public string GetKeyboardLocalized()
    {
        return GetLocalizedUIText(KeyboardKey);
    }

    public string GetGamepadLocalized()
    {
        return GetLocalizedUIText(GamepadKey);
    }

    public void ChangeToGamepad()
    {
        OnControllerChange?.Invoke(true);
    }

    public void ChangeToKeyboard()
    {
        OnControllerChange?.Invoke(false);
    }

    public void FillConfirmationPanel(string confirmationMessage, UnityAction OnSubmit, UnityAction OnCancel)
    {
        _confirmation.FillConfirmationPanel(confirmationMessage, OnSubmit, OnCancel);
    }

    public void ShowConfirmationPanel(GameObject currentSelectedGameObject)
    {
        _confirmation.PreviousSelected = EventSystem.current.currentSelectedGameObject;
        _confirmation.gameObject.SetActive(true);
    }

    public void SaveAndQuit()
    {
        SaveGameManager.Instance.SaveSessionDataToFile();
        //Can create confirmation message if we want
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += _fadeIncrement;
            _loadingScreen.alpha = Mathf.Clamp01(elapsedTime / _fadeDuration);
            yield return null;
        }
        Time.timeScale = 1f;
    }
}
