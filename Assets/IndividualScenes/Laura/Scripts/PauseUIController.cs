using UnityEngine;
using UnityEngine.EventSystems;

public class PauseUIController : MonoBehaviour
{
    [SerializeField] private ConfirmationController _confirmationContoller;
    private GameObject _previousSelected;

    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _optionsFirtsSelected;
    [SerializeField] private GameObject _controlsPanel;
    [SerializeField] private GameObject _controlsFirtsSelected;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_previousSelected);
    }

    public void GenerateConfirmationPanel()
    {
        UIManager.Instance.FillConfirmationPanel(UIManager.Instance.GetLocalizedUIText("NewGameConfirmation"),
            () => {
                /*On Confirmation*/
            },
            () => {
                EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            });
        UIManager.Instance.ShowConfirmationPanel(EventSystem.current.currentSelectedGameObject);

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
}
