using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    private GameObject _previousSelected;
    [SerializeField] private GameObject _continueButton;


    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_previousSelected);
    }

    private void Start()
    {
        HideContinue();
    }

    public void HideContinue()
    {
        //Session not started and data not present
        if (!SaveGameManager.IsSessionStarted && !SaveGameManager.Instance.IsDataSavedInFile())
        {
            _continueButton.SetActive(false);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(_continueButton);
        }
    }

    public void StartNewGame()
    {
        Debug.Log("Start new Game");
        //If there is an occupied slot, ask for confirmation
        if (SaveGameManager.Instance.IsDataSavedInFile())
        {
            GenerateNewSaveConfirmationPanel();
        }
        else
        {
            //SceneManager.LoadScene("HUB");
            SaveGameManager.Instance.ResetSessionData();
            SceneManager.LoadScene("IntroVideoScene");
        }
    }

    public void GenerateNewSaveConfirmationPanel()
    {
        UIManager.Instance.FillConfirmationPanel(UIManager.Instance.GetLocalizedUIText("NewGameConfirmation"),
            () => {
                /*Delete previous saved data values*/
                //Change to Intro Video Scene
                SaveGameManager.Instance.DeleteSaveData();
                SaveGameManager.Instance.ResetSessionData();
                SceneManager.LoadScene("IntroVideoScene");
            },
            () => {
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            });
        UIManager.Instance.ShowConfirmationPanel(EventSystem.current.currentSelectedGameObject);
    }

    public void Continue()
    {
        StartCoroutine(UIManager.Instance.LoadScene("HUB"));
    }

    public void Options()
    {
        _previousSelected = EventSystem.current.currentSelectedGameObject;
        gameObject.SetActive(false);
        UIManager.Instance.EnableOptionsMenu();
    }

    public void Credits()
    {
        _previousSelected = EventSystem.current.currentSelectedGameObject;
        gameObject.SetActive(false);
        UIManager.Instance.EnableCreditsMenu();
    }

    public void Quit()
    {
        UIManager.Instance.SaveAndQuit();
    }

}
