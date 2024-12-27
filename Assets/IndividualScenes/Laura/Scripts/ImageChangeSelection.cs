using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageChangeSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Image _image;
    [SerializeField] private Sprite _primaryImage;
    [SerializeField] private Sprite _secondaryImage;

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
        _image.sprite = _primaryImage;
    }
}
