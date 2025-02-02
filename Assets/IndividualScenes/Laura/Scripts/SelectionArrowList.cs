using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SelectionArrowList : SelectLeftRight
{
    [SerializeField] private TextMeshProUGUI _optionText;
    [SerializeField] private Image _leftArrow;
    [SerializeField] private Image _rightArrow;

    [SerializeField] private Option[] _optionsArray;
    private int _currentIndex;

    public void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnEnable()
    {
        OnLoadedComponent?.Invoke();
    }

    override public void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir == MoveDirection.Left)
        {
            _currentIndex++;
            Apply();
        }
        else if (eventData.moveDir == MoveDirection.Right)
        {
            _currentIndex--;
            Apply();
        }
    }

    public void Apply()
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.select);
        int index = Math.Abs(_currentIndex) % _optionsArray.Length;
        _optionsArray[index].OnOptionSelected?.Invoke();
    }

    override public void OnCancel(BaseEventData eventData)
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.goBack);
        Deactivate();
    }

    override public void OnSubmit(BaseEventData eventData)
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.goBack);
        Deactivate();
    }

    override public void Activate(GameObject previousSelected)
    {
        base.Activate(previousSelected);
        ShowArrows(true);
        //If we want to do something else
    }

    override public void Deactivate()
    {
        base.Deactivate();
        ShowArrows(false);
        //If we want to do something else
    }

    public void FillResolutionOptions()
    {
        if (_optionsArray != null)
        {
            return;
        }

        int resolutionSize = UIManager.AvailableResolutions.Length;
        _optionsArray = new Option[resolutionSize];

        string currentRes = FormatResolution(Screen.currentResolution);
        for (int index = 0; index < resolutionSize; index++)
        {
            Resolution resolution = UIManager.AvailableResolutions[index];
            string formattedResolution = FormatResolution(resolution);

            _optionsArray[index] = new Option(formattedResolution, () =>
            {
                _optionText.text = formattedResolution;
                Screen.SetResolution(resolution.width, resolution.height, true);
                SaveGameManager.Instance.SessionData.SessionResolution.Width = resolution.width;
                SaveGameManager.Instance.SessionData.SessionResolution.Height = resolution.height;
            });
            //Change index to current resolution
            if (SaveGameManager.Instance.SessionData.SessionResolution.Width == resolution.width && SaveGameManager.Instance.SessionData.SessionResolution.Height == resolution.height)
            {
                _currentIndex = index;
            }
        }
        _optionText.text = _optionsArray[_currentIndex].Value;
    }

    private string FormatResolution(Resolution resolution)
    {
        return String.Format("{0}x{1} {2}H", resolution.width, resolution.height, Math.Floor(resolution.refreshRateRatio.value));
    }

    public void FillLanguageOptions()
    {
        if (_optionsArray != null)
        {
            return;
        }

        int localeSize = UIManager.AvailableLocales.Length;
        _optionsArray = new Option[localeSize];

        LocalizeStringEvent localizedObj = _optionText.GetComponent<LocalizeStringEvent>();
        localizedObj.SetTable(UIManager.Instance.UITextTable.TableCollectionName);
        localizedObj.OnUpdateString.AddListener((string textChanged) => CallingChange(textChanged));

        for (int index = 0; index < localeSize; index++)
        {
            Locale locale = UIManager.AvailableLocales[index];

            _optionsArray[index] = new Option(locale.Identifier.Code, () =>
            {
                LocalizationSettings.SelectedLocale = locale;
                localizedObj.SetEntry(locale.Identifier.Code);
                UIManager.Instance.UpdateLanguage();
                SaveGameManager.Instance.SessionData.Locale = locale.Identifier.Code;
            });

            if (SaveGameManager.Instance.SessionData.Locale == locale.Identifier.Code)
            {
                _currentIndex = index;
            }
        }
        _optionText.text = UIManager.Instance.GetLocalizedUIText(_optionsArray[_currentIndex].Value);
    }

    public void CallingChange(string text)
    {
        _optionText.text = text;
    }
    public void ShowArrows(bool show)
    {
        Color _leftTmpColor = _leftArrow.color;
        Color _rightTmpColor = _rightArrow.color;
        if (show)
        {
            _leftTmpColor.a = 1f;
            _rightTmpColor.a = 1f;
        }
        else{
            _leftTmpColor.a = 0f;
            _rightTmpColor.a = 0f;
        }
        _leftArrow.color = _leftTmpColor;
        _rightArrow.color = _rightTmpColor;
    }
    public override void OnValueChanged()
    {
        //We are not doing anything for now
    }
}
