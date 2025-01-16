using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //Menus
    [SerializeField] private MainMenuUIController _mainMenu;
    [SerializeField] private OptionsUIController _optionsMenu;
    [SerializeField] private CreditsUIController _creditsMenu;

    //Confirmation
    [SerializeField] private ConfirmationController _confirmation;

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

    //Video
    //TODO: Can be separated on another script if necessary
    [SerializeField] private VideoPlayer _videoPlayer;

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
        AvailableResolutions = Screen.resolutions;
        //Localization initialization
        AvailableLocales = LocalizationSettings.AvailableLocales.Locales.ToArray();
        _localeIndex = 0;
        LocalizationSettings.SelectedLocale = AvailableLocales[_localeIndex];
        UITextTable = LocalizationSettings.StringDatabase.GetTable(UITableName);
        //LocalizationSettings.InitializationOperation.Completed += FinishLoadingLocalization;
    }

    public void UpdateLanguage()
    {
        UITextTable = LocalizationSettings.StringDatabase.GetTable(UITableName);
    }

    /*private void FinishLoadingLocalization(AsyncOperationHandle<LocalizationSettings> handle)
    {
        _localeIndex = 0;
        LocalizationSettings.SelectedLocale = _availableLocales[_localeIndex];
        _uiTextsTable = LocalizationSettings.StringDatabase?.GetTable(UITableName);
    }*/

    public String GetLocalizedUIText(String localizedKey)
    {
        return UITextTable.GetEntry(localizedKey).LocalizedValue;
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

    public void PlayVideo(VideoClip videoClip)
    {
        _videoPlayer.clip = videoClip;
        _videoPlayer.Play();
    }

}
