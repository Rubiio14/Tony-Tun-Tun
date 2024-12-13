using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SubmitableColorChange : MonoBehaviour, ISubmitHandler, ICancelHandler
{
    private Image _image;
    [SerializeField] private Color _primaryColor;
    [SerializeField] private Color _secondaryColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        _image.color = _secondaryColor;
    }

    public void OnCancel(BaseEventData eventData)
    {
        _image.color = _primaryColor;
    }
}
