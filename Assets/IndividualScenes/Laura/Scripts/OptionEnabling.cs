using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OptionEnabling : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, ICancelHandler
{
    private Image _image;
    [SerializeField] private Sprite _primaryImage;
    [SerializeField] private Sprite _secondaryImage;
    //Must implement IActivate, can also search for it, if they are going to be prefabs
    [SerializeField] private GameObject _elementToReference;
    private IActivate _elementToActivate;

    public UnityEvent OnCancellation;

    public void Awake()
    {
        _image = GetComponent<Image>();
        if(!_elementToReference.TryGetComponent<IActivate>(out _elementToActivate))
        {
            #if UNITY_EDITOR
                Debug.Log("Missing interface");
            #endif
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        ChangeImageTo(_secondaryImage);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (_elementToActivate.Submitted)
        {
            return;
        }
        ChangeImageTo(_primaryImage);
    }

    public void ChangeImageTo(Sprite image)
    {
        _image.sprite = image;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        _elementToActivate.Submitted = true;
        _elementToActivate.Activate(gameObject);
    }

    public void OnCancel(BaseEventData eventData)
    {
        //Return to main menu, animations if we want
        ChangeImageTo(_primaryImage);
        OnCancellation?.Invoke();
    }

}
