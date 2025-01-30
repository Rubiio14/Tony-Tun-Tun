using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsUIController : MonoBehaviour, ISubmitHandler, ICancelHandler
{
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private void GoBackToMainMenu()
    {
        UIManager.Instance.DisableCreditsMenu();
        UIManager.Instance.EnableMainMenu();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.goBack);
        GoBackToMainMenu();
    }
    public void OnCancel(BaseEventData eventData)
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.goBack);
        GoBackToMainMenu();
    }
}
