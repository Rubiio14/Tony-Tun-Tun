using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance {  get; private set; }

    //Gradient
    public TMP_ColorGradient GreenGradient;
    public TMP_ColorGradient PinkGradient;

    [SerializeField]
    private ConfirmationController _confirmationPanel;
    private GameObject _previousSelected;

    private SaveData _savedData;

    //Localization
    private List<Locale> _availableLocales;
    private int _localeIndex;
    private StringTable _uiTextsTable;


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

    public void Start()
    {
        _savedData = GameManager.Instance.LoadData();
    }

    private void FinishLoadingLocalization(AsyncOperationHandle<LocalizationSettings> handle)
    {
        _localeIndex = 0;
        LocalizationSettings.SelectedLocale = _availableLocales[_localeIndex];
        _uiTextsTable = LocalizationSettings.StringDatabase?.GetTable("UI");
    }

    public String GetLocalizedUIText(String localizedKey)
    {
        return _uiTextsTable.GetEntry(localizedKey)?.GetLocalizedString();
    }

    public void StartNewGame()
    {
        Debug.Log("Start new Game");
        //If there is an occupied slot, ask for confirmation
        //if (_savedData != null)
        //{
            GenerateNewSaveConfirmationPanel();


        //}


    }

    public void Options()
    {

    }

    public void GenerateNewSaveConfirmationPanel() {

        _previousSelected = EventSystem.current.currentSelectedGameObject;
        _confirmationPanel.FillForNewGameConfirmation(
            () => { 
                /*Delete previous saved data values*/ 
            }, 
            () => { 
                EventSystem.current.SetSelectedGameObject(_previousSelected);  
                _confirmationPanel.gameObject.SetActive(false);
            });
        _confirmationPanel.gameObject.SetActive(true);
    }

    public void ShowReturnToMainMenuConfirmation()
    {

    }

    public void ShowReturnToHubConfirmation()
    {

    }

    public void ReturnToHub()
    {

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
