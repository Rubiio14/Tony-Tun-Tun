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
    
    //Scene
    [SerializeField] private float _delayForSceneChange;

    //Localization
    public static Locale[] AvailableLocales { get; private set; }
    private int _localeIndex;
    public StringTable UITextTable { get; private set; }

    //Resolution
    public static Resolution[] AvailableResolutions { get; private set; }

    //Controller
    //TODO: Add removal of listeners at some point
    public event Action<bool> OnControllerChange;

    //Global variables for UI localization
    private string UITableName = "UI";
    private string KeyboardKey = "Keyboard";
    private string GamepadKey = "Gamepad";

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

        //Volume initialization
        FMODAudioManager.instance.LoadVolumes();

        UITextTable = LocalizationSettings.StringDatabase.GetTable(UITableName);
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
        yield return new WaitForSeconds(_delayForSceneChange);
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

    public void EnableLevelPauseMenu()
    {
        _pauseMenu.gameObject.SetActive(true);
    }

    public void DisableLevelPauseMenu()
    {
        LevelManager.Instance.ReturnControlsToPlayer();
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
}
