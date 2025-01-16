using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    private GameObject _previousSelected;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_previousSelected);
    }

    public void StartNewGame()
    {
        Debug.Log("Start new Game");
        //If there is an occupied slot, ask for confirmation
        //if (GameManager.Instance.SavedData != null)
        //{
            GenerateNewSaveConfirmationPanel();
        //}

    }

    public void GenerateNewSaveConfirmationPanel()
    {
        UIManager.Instance.FillConfirmationPanel(UIManager.Instance.GetLocalizedUIText("NewGameConfirmation"),
            () => {
                /*Delete previous saved data values*/
                //Change to Intro Video Scene
                SceneManager.LoadScene("IntroVideoScene");
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            },
            () => {
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            });
        UIManager.Instance.ShowConfirmationPanel(EventSystem.current.currentSelectedGameObject);
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
