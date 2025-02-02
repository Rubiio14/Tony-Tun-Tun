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

    private void Start()
    {
        _imageReference = GetComponent<Image>();
    }

    private void OnEnable()
    {
        UIManager.Instance.OnControllerChange += OnControllerChange;
    }

    private void OnDisable()
    {
        UIManager.Instance.OnControllerChange -= OnControllerChange;
    }

    private void OnControllerChange(bool controller)
    {
        _imageReference.sprite = controller ? _gamepadSprite : _keyboardSprite;
    }

}
