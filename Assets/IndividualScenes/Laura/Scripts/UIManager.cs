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
    public Locale[] AvailableLocales { get; private set; }
    private int _localeIndex;
    public StringTable UITextTable { get; private set; }

    //Resolution
    public Resolution[] AvailableResolutions { get; private set; }

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
        if(AvailableResolutions == null)
        {
            //TODO: Check in testing
            AvailableResolutions = Screen.resolutions.Where(res => {
                Debug.Log($"Resolution: {res.width}x{res.height} ({res.refreshRateRatio})");
                if (res.refreshRateRatio.value > 59)
                    if (res.width == 1920 && res.height == 1080
                    || res.width == 1366 && res.height == 768
                    || res.width == 2560 && res.height == 1440
                    || res.width == 3840 && res.height == 2160)
                    {
                        return true;
                    }
                return false;

            }).ToArray();
        }
        //Localization initialization
        AvailableLocales = LocalizationSettings.AvailableLocales.Locales.ToArray();
        _localeIndex = 0;
        LocalizationSettings.SelectedLocale = AvailableLocales[_localeIndex];
        UITextTable = LocalizationSettings.StringDatabase.GetTable(UITableName);
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
        _pauseMenu.gameObject.SetActive(false);
        HubManager.Instance.ReturnControlsToPlayer();
    }

    public void EnableLevelPauseMenu()
    {
        _pauseMenu.gameObject.SetActive(true);
    }

    public void DisableLevelPauseMenu()
    {
        _pauseMenu.gameObject.SetActive(false);
        //LevelManager.Instance.ReturnControlsToPlayer();
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
