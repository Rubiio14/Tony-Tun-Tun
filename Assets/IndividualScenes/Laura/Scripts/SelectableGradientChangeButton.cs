using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SelectableGradientChangeButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private TextMeshProUGUI _buttonText;

    //Gradients
    [SerializeField] private TMP_ColorGradient _primaryGradient;
    [SerializeField] private TMP_ColorGradient _secondaryGradient;

    public void Awake()
    {
        _buttonText = GetComponent<TextMeshProUGUI>();
    }

    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {
        _buttonText.colorGradientPreset = _secondaryGradient;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _buttonText.colorGradientPreset = _primaryGradient;
    }
}
