using UnityEngine;
using UnityEngine.EventSystems;

public class PauseUIController : MonoBehaviour
{
    [SerializeField] private GameObject _firstToSelect;

    private GameObject _previousSelected;

    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _optionsFirtsSelected;
    [SerializeField] private GameObject _controlsPanel;
    [SerializeField] private GameObject _controlsFirtsSelected;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_previousSelected);
    }

    public void SelectFirstElement()
    {
        EventSystem.current.SetSelectedGameObject(_firstToSelect);
    }

    public void GenerateBackToHubConfirmationPanel()
    {
        UIManager.Instance.FillConfirmationPanel(UIManager.Instance.GetLocalizedUIText("BackToHubConfirmation"),
            () => {
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.confirm);
                FMODAudioManager.instance.StopMusic();
                StartCoroutine(UIManager.Instance.LoadScene("HUB"));
            },
            () => {
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.goBack);
                EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            });
        UIManager.Instance.ShowConfirmationPanel(EventSystem.current.currentSelectedGameObject);

    }

    public void GenerateBackToMainMenuConfirmationPanel()
    {
        UIManager.Instance.FillConfirmationPanel(UIManager.Instance.GetLocalizedUIText("BackToMainMenuConfirmation"),
            () => {
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.confirm);
                FMODAudioManager.instance.StopMusic();
                StartCoroutine(UIManager.Instance.LoadScene("MainMenu"));
            },
            () => {
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.goBack);
                EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            });
        UIManager.Instance.ShowConfirmationPanel(EventSystem.current.currentSelectedGameObject);

    }

    public void GenerateSaveAndExitConfirmationPanel()
    {
        UIManager.Instance.FillConfirmationPanel(UIManager.Instance.GetLocalizedUIText("SaveAndQuitConfirmation"),
            () => {
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.confirm);
                UIManager.Instance.SaveAndQuit();
            },
            () => {
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.goBack);
                EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            });
        UIManager.Instance.ShowConfirmationPanel(EventSystem.current.currentSelectedGameObject);

    }

    public void ContinueInHUB()
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.confirm);
        _previousSelected = EventSystem.current.currentSelectedGameObject;
        gameObject.SetActive(false);
        UIManager.Instance.DisableHUBPauseMenu();
    }

    public void ContinueInLevel()
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.confirm);
        _previousSelected = EventSystem.current.currentSelectedGameObject;
        gameObject.SetActive(false);
        UIManager.Instance.DisableLevelPauseMenu();
    }

    public void Options()
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.confirm);
        _previousSelected = EventSystem.current.currentSelectedGameObject;
        _controlsPanel.SetActive(false);
        _optionsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_optionsFirtsSelected);
    }

    public void DisableOptions()
    {
        _optionsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_previousSelected);
    }

    public void Controls()
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.confirm);
        _previousSelected = EventSystem.current.currentSelectedGameObject;
        _optionsPanel.SetActive(false);
        _controlsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_controlsFirtsSelected);
    }

    public void DisableControls()
    {
        _controlsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_previousSelected);
    }

    public void BackToHUB()
    {
        GenerateBackToHubConfirmationPanel();
    }

    public void BackToMainMenu()
    {
        GenerateBackToMainMenuConfirmationPanel();
    }
}
