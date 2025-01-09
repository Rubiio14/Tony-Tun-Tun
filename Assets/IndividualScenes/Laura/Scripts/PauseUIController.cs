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

        _confirmationContoller.PreviousSelected = EventSystem.current.currentSelectedGameObject;
        _confirmationContoller.FillForNewGameConfirmation(
            () => {
                //On Submit
            },
            () => {
                //On Cancel
                EventSystem.current.SetSelectedGameObject(_confirmationContoller.PreviousSelected);
                _confirmationContoller.gameObject.SetActive(false);
            });
        _confirmationContoller.gameObject.SetActive(true);
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
