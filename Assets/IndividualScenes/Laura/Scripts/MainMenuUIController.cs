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
        if (!SaveGameManager.Instance.IsDataSaved())
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
        if (SaveGameManager.Instance.IsDataSaved())
        {
            GenerateNewSaveConfirmationPanel();
        }
        else
        {
            SceneManager.LoadScene("HUB");
            //SceneManager.LoadScene("IntroVideoScene");
        }
    }

    public void GenerateNewSaveConfirmationPanel()
    {
        UIManager.Instance.FillConfirmationPanel(UIManager.Instance.GetLocalizedUIText("NewGameConfirmation"),
            () => {
                /*Delete previous saved data values*/
                //Change to Intro Video Scene
                SaveGameManager.Instance.DeleteSaveData();
                SceneManager.LoadScene("IntroVideoScene");
            },
            () => {
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            });
        UIManager.Instance.ShowConfirmationPanel(EventSystem.current.currentSelectedGameObject);
    }

    public void Continue()
    {
        //_firstTimeInHUB ??
        SceneManager.LoadScene("HUB");
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
        //Can create confirmation message if we want
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

}
