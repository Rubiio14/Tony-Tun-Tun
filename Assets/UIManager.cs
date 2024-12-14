using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;
using System;
using UnityEngine.Localization.Settings;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //Menus
    [SerializeField] private MainMenuUIController MainMenu;
    [SerializeField] private OptionsUIController OptionsMenu;
    //[SerializeField] private CreditsUIController CreditsMenu;

    //Confirmation
    [SerializeField] private ConfirmationController Confirmation;

    //Localization
    public Locale[] AvailableLocales { get; private set; }
    private int _localeIndex;
    public StringTable UITextTable { get; private set; }

    //Resolution
    public Resolution[] AvailableResolutions { get; private set; }


    //Controller
    public event Action<bool> OnControllerChange;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        AvailableResolutions = Screen.resolutions;
        //Localization initialization
        AvailableLocales = LocalizationSettings.AvailableLocales.Locales.ToArray();
        _localeIndex = 0;
        LocalizationSettings.SelectedLocale = AvailableLocales[_localeIndex];
        UITextTable = LocalizationSettings.StringDatabase?.GetTable("UI");
        //LocalizationSettings.InitializationOperation.Completed += FinishLoadingLocalization;
    }
    void Start()
    {

    }

    /*private void FinishLoadingLocalization(AsyncOperationHandle<LocalizationSettings> handle)
    {
        _localeIndex = 0;
        LocalizationSettings.SelectedLocale = _availableLocales[_localeIndex];
        _uiTextsTable = LocalizationSettings.StringDatabase?.GetTable("UI");
    }*/

    public String GetLocalizedUIText(String localizedKey)
    {
        return UITextTable.GetEntry(localizedKey)?.GetLocalizedString();
    }

    public void EnableMainMenu()
    {
        MainMenu.gameObject.SetActive(true);
    }

    public void EnableOptionsMenu()
    {
        OptionsMenu.gameObject.SetActive(true);
    }

    public void DisableOptionsMenu()
    {
        OptionsMenu.gameObject.SetActive(false);
    }

    public void EnableCreditsMenu()
    {
        //CreditsMenu.gameObject.SetActive(true);
    }

    public void ChangeToGamepad()
    {
        OnControllerChange?.Invoke(true);
    }

    public void ChangeToKeyboard()
    {
        OnControllerChange?.Invoke(false);
    }

    
}
