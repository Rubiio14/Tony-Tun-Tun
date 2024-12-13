using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsUIController : MonoBehaviour, ISubmitHandler, ICancelHandler
{
    public GameObject FirstSelected;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(FirstSelected);
    }

    public void OnSubmit(BaseEventData eventData)
    {

        //throw new System.NotImplementedException();
    }

    public void OnCancel(BaseEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
