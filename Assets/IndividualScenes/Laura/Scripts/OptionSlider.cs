using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OptionSlider : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, ICancelHandler
{
    private Image _image;
    [SerializeField] private Sprite _primaryImage;
    [SerializeField] private Sprite _secondaryImage;
    [SerializeField] private SliderRange _sliderRange;
    public bool Submitted { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        _image.sprite = _secondaryImage;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (Submitted)
        {
            return;
        }
        _image.sprite = _primaryImage;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        _sliderRange.Enable(this);
        Submitted = true;
        EventSystem.current.SetSelectedGameObject(_sliderRange.gameObject);
    }

    public void OnCancel(BaseEventData eventData)
    {
        //Return to main menu
    }

}
