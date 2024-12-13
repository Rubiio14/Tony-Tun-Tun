using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuUIController : MonoBehaviour
{
    private ConfirmationController _confirmationContoller;
    private GameObject _previousSelected;

    private void Start()
    {
        _confirmationContoller = UIManager.Instance.Confirmation;
    }

    private void OnEnable()
    {
        //EventSystem.current.SetSelectedGameObject(_previousSelected);
    }

    public void StartNewGame()
    {
        Debug.Log("Start new Game");
        //If there is an occupied slot, ask for confirmation
        //if (GameManager.SavedData != null)
        //{
            GenerateNewSaveConfirmationPanel();
        //}

    }

    public void GenerateNewSaveConfirmationPanel()
    {

        _confirmationContoller.PreviousSelected = EventSystem.current.currentSelectedGameObject;
        _confirmationContoller.FillForNewGameConfirmation(
            () => {
                /*Delete previous saved data values*/
            },
            () => {
                EventSystem.current.SetSelectedGameObject(_confirmationContoller.PreviousSelected);
                _confirmationContoller.gameObject.SetActive(false);
            });
        _confirmationContoller.gameObject.SetActive(true);
    }

    public void Options()
    {
        _previousSelected = EventSystem.current.currentSelectedGameObject;
        UIManager.Instance.OptionsMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Credits()
    {

    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
