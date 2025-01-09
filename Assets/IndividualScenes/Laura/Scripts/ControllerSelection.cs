using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ControllerSelection : MonoBehaviour, IMoveHandler, ICancelHandler, ISubmitHandler
{
    [SerializeField] protected TextMeshProUGUI _optionText;

    [SerializeField] private GameObject _keyboardDiv;
    [SerializeField] private GameObject _gamepadDiv;

    [SerializeField] protected Option[] _optionsArray;
    protected int _currentIndex;

    public UnityEvent OnCancellation;

    public void OnEnable()
    {
        FillControllerOptions();
    }

    public void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir == MoveDirection.Left)
        {
            _currentIndex++;
            Apply();
        }
        else if (eventData.moveDir == MoveDirection.Right)
        {
            _currentIndex--;
            Apply();
        }
    }

    public void Apply()
    {
        int index = Math.Abs(_currentIndex) % _optionsArray.Length;
        _optionsArray[index].OnOptionSelected?.Invoke();
    }

    public void FillControllerOptions()
    {
        _optionsArray = new Option[2];
        String keyboardName = UIManager.Instance.GetLocalizedUIText("Keyboard");
        _optionsArray[0] = new Option(keyboardName, () =>
        {
            //Change image to keyboard one
            _keyboardDiv.SetActive(true);
            _gamepadDiv.SetActive(false);
            _optionText.text = keyboardName;
        });

        String gamepadName = UIManager.Instance.GetLocalizedUIText("Gamepad");
        _optionsArray[1] = new Option(gamepadName, () =>
        {
            //Change image to keyboard one
            _gamepadDiv.SetActive(true);
            _keyboardDiv.SetActive(false);
            _optionText.text = gamepadName;
        });

        Apply();
    }
    
    public void OnCancel(BaseEventData eventData)
    {
        //_gamepadDiv.SetActive(false);
        //_keyboardDiv.SetActive(false);
        OnCancellation?.Invoke();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        //_gamepadDiv.SetActive(false);
        //_keyboardDiv.SetActive(false);
        OnCancellation?.Invoke();
    }

}
