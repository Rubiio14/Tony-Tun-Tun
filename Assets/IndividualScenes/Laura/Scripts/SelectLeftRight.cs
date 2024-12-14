using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SelectLeftRight : MonoBehaviour, IMoveHandler, ICancelHandler, ISubmitHandler, IActivate
{
    protected Image _image;
    [SerializeField] private Color _primaryColor;
    [SerializeField] private Color _secondaryColor;

    private bool _submitted;
    public bool Submitted { get => _submitted; set => _submitted = value; }

    private GameObject _previousSelected;
    
    public UnityEvent OnLoadedComponent;

    public abstract void OnMove(AxisEventData eventData);

    public abstract void OnValueChanged();

    public abstract void OnCancel(BaseEventData eventData);

    public abstract void OnSubmit(BaseEventData eventData);

    public virtual void Activate(GameObject previousSelected)
    {
        _image.color = _secondaryColor;
        _previousSelected = previousSelected;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
    public virtual void Deactivate()
    {
        _image.color = _primaryColor;
        _submitted = false;
        EventSystem.current.SetSelectedGameObject(_previousSelected);
    }
}
