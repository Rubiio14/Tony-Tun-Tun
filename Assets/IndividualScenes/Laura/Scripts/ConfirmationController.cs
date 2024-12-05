using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ConfirmationController : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, ICancelHandler
{
    [SerializeField]
    private TextMeshProUGUI _confirmationTxt;

    private UnityAction _onConfirmation;
    private UnityAction _onCancelation;

    public void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void FillForNewGameConfirmation(UnityAction ConfirmAction, UnityAction CancelationAction)
    {
        _confirmationTxt.text = UIController.Instance.GetLocalizedUIText("NewGameConfirmation");
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
