using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SliderRange : MonoBehaviour, IMoveHandler, ICancelHandler, ISubmitHandler { 

    private Image _image;
    [SerializeField] private Color _primaryColor;
    [SerializeField] private Color _secondaryColor;

    private OptionSlider _previousSelected;
    private Slider _slider;
    [SerializeField] private float _increment;
    [SerializeField] private TextMeshProUGUI _UISoundText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _image = GetComponent<Image>();
        _slider = transform.GetChild(0).GetComponent<Slider>();
    }

    public void Enable(OptionSlider optionSlider)
    {
        _image.color = _secondaryColor;
        _previousSelected = optionSlider;
    }

    public void Disable()
    {
        _image.color = _primaryColor;
    }

    public void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir == MoveDirection.Left)
        {
            _slider.value -= _increment;
        }

        if(eventData.moveDir == MoveDirection.Right)
        {
            _slider.value += _increment;
        }
    }

    public string SoundValue()
    {
        return _slider.value.ToString();
    }

    public void OnCancel(BaseEventData eventData)
    {
        Disable();
        _previousSelected.Submitted = false;
        EventSystem.current.SetSelectedGameObject(_previousSelected.gameObject);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Disable();
        _previousSelected.Submitted = false;
        EventSystem.current.SetSelectedGameObject(_previousSelected.gameObject);
    }
}
