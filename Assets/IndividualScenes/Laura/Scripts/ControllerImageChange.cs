using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ControllerImageChange : MonoBehaviour
{
    private Image _imageReference;
    [SerializeField]
    private Sprite _gamepadSprite;
    [SerializeField]
    private Sprite _keyboardSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _imageReference = GetComponent<Image>();
        UIManager.Instance.OnControllerChange += OnControllerChange;
    }

    private void OnControllerChange(bool controller)
    {
        _imageReference.sprite = controller ? _gamepadSprite : _keyboardSprite;
    }

    private void OnDisable()
    {
        UIManager.Instance.OnControllerChange -= OnControllerChange;
    }
}
