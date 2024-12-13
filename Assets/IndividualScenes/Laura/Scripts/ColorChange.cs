using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorChange : MonoBehaviour
{
    private Image _image;
    [SerializeField] private Color _newColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void ChangeColor(BaseEventData eventData)
    {
        _image.color = _newColor;
    }
    
}
