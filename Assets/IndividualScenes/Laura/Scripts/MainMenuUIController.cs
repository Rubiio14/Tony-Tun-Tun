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
        if (!SaveGameManager.IsSessionStarted && !SaveGameManager.Instance.IsDataSavedInFile())
        {
            _continueButton.SetActive(false);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(_continueButton);
        }
        FMODAudioManager.instance.InitializeMusic(FMODEvents.instance.hubMusic);
    }

    public void StartNewGame()
    {
        Debug.Log("Start new Game");
        //If there is save data, ask for confirmation
        if (SaveGameManager.Instance.IsDataSavedInFile())
        {
            GenerateNewSaveConfirmationPanel();
        }
        else
        {
            FMODAudioManager.instance.StopMusic();
            SaveGameManager.Instance.ResetSessionData();
            SceneManager.LoadScene("IntroVideoScene");
        }
    }

    public void GenerateNewSaveConfirmationPanel()
    {
        UIManager.Instance.FillConfirmationPanel(UIManager.Instance.GetLocalizedUIText("NewGameConfirmation"),
            () => {
                FMODAudioManager.instance.StopMusic();
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.confirm);
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
        FMODAudioManager.instance.StopMusic();
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.confirm);
        StartCoroutine(UIManager.Instance.LoadScene("HUB"));
    }

    public void Options()
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.confirm);
        _previousSelected = EventSystem.current.currentSelectedGameObject;
        gameObject.SetActive(false);
        UIManager.Instance.EnableOptionsMenu();
    }

    public void Credits()
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.confirm);
        _previousSelected = EventSystem.current.currentSelectedGameObject;
        gameObject.SetActive(false);
        UIManager.Instance.EnableCreditsMenu();
    }

    public void Quit()
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.confirm);
        if (SaveGameManager.HUBVisitedInCurrentSession)
        {
            UIManager.Instance.SaveAndQuit();
        }
        else
        {
            Application.Quit();
        }
    }

}
