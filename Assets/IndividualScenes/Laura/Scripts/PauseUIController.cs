using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseUIController : MonoBehaviour
{
    private GameObject _previousSelected;

    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _optionsFirtsSelected;
    [SerializeField] private GameObject _controlsPanel;
    [SerializeField] private GameObject _controlsFirtsSelected;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_previousSelected);
    }

    public void GenerateBackToHubConfirmationPanel()
    {
        UIManager.Instance.FillConfirmationPanel(UIManager.Instance.GetLocalizedUIText("BackToHubConfirmation"),
            () => {
                StartCoroutine(UIManager.Instance.LoadScene("HUB"));
            },
            () => {
                EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            });
        UIManager.Instance.ShowConfirmationPanel(EventSystem.current.currentSelectedGameObject);

    }

    public void GenerateBackToMainMenuConfirmationPanel()
    {
        UIManager.Instance.FillConfirmationPanel(UIManager.Instance.GetLocalizedUIText("BackToMainMenuConfirmation"),
            () => {
                StartCoroutine(UIManager.Instance.LoadScene("MainMenu"));
            },
            () => {
                EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            });
        UIManager.Instance.ShowConfirmationPanel(EventSystem.current.currentSelectedGameObject);

    }

    public void GenerateSaveAndExitConfirmationPanel()
    {
        UIManager.Instance.FillConfirmationPanel(UIManager.Instance.GetLocalizedUIText("SaveAndQuitConfirmation"),
            () => {
                /*On Confirmation*/
                UIManager.Instance.SaveAndQuit();
            },
            () => {
                EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            });
        UIManager.Instance.ShowConfirmationPanel(EventSystem.current.currentSelectedGameObject);

    }

    public void ContinueInHUB()
    {
        _previousSelected = EventSystem.current.currentSelectedGameObject;
        gameObject.SetActive(false);
        UIManager.Instance.DisableHUBPauseMenu();
    }

    public void ContinueInLevel()
    {
        _previousSelected = EventSystem.current.currentSelectedGameObject;
        gameObject.SetActive(false);
        UIManager.Instance.DisableLevelPauseMenu();
    }

    public void Options()
    {
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
