using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SelectableTextButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private TextMeshProUGUI _buttonText;

    private void Start()
    {
        _buttonText = GetComponent<TextMeshProUGUI>();
    }

    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {
        _buttonText.colorGradientPreset = UIController.Instance.PinkGradient;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _buttonText.colorGradientPreset = UIController.Instance.GreenGradient;
    }
}
