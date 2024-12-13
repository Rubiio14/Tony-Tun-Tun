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
    public MainMenuUIController MainMenu;
    public OptionsUIController OptionsMenu;

    //Confirmation
    public ConfirmationController Confirmation;

    //Localization
    private List<Locale> _availableLocales;
    private int _localeIndex;
    private StringTable _uiTextsTable;

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
        //Localization initialization
        _availableLocales = LocalizationSettings.AvailableLocales.Locales;
        _localeIndex = 0;
        LocalizationSettings.SelectedLocale = _availableLocales[_localeIndex];
        _uiTextsTable = LocalizationSettings.StringDatabase?.GetTable("UI");
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
        return _uiTextsTable.GetEntry(localizedKey)?.GetLocalizedString();
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
