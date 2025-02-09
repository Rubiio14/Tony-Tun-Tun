using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ConfirmationController : MonoBehaviour, IDeselectHandler, ISubmitHandler, ICancelHandler
{
    [SerializeField]
    private TextMeshProUGUI _confirmationTxt;
    public GameObject PreviousSelected { get; set; }

    private UnityAction _onConfirmation;
    private UnityAction _onCancelation;

    public void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnDisable()
    {
        if (PreviousSelected && EventSystem.current)
        {
            EventSystem.current.SetSelectedGameObject(PreviousSelected);
        }
    }

    public void FillConfirmationPanel(string confirmMessage, UnityAction ConfirmAction, UnityAction CancelationAction)
    {
        _confirmationTxt.text = confirmMessage;
        _onConfirmation = ConfirmAction;
        _onCancelation = CancelationAction;
    }

    public void CleanConfirmationPanel()
    {
        _confirmationTxt.text = "";
        _onConfirmation = null;
        _onCancelation = null;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        CleanConfirmationPanel();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.select);
        _onConfirmation?.Invoke();
    }

    public void OnCancel(BaseEventData eventData)
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.goBack);
        _onCancelation?.Invoke();
    }
}
