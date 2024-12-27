using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsUIController : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelected;

    public void SelectFirst()
    {
        EventSystem.current.SetSelectedGameObject(_firstSelected);
    }
    
}
