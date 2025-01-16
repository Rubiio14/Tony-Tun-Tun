using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ConfirmationController : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, ICancelHandler
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
        EventSystem.current.SetSelectedGameObject(PreviousSelected);
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

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Changing to confirmation canvas");
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("Deselecting confirmation canvas");
        CleanConfirmationPanel();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("Clicking submit");
        _onConfirmation?.Invoke();
    }

    public void OnCancel(BaseEventData eventData)
    {
        Debug.Log("Clicking cancel");
        _onCancelation?.Invoke();
    }
}
