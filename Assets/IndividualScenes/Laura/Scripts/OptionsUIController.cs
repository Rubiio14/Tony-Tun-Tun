using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsUIController : MonoBehaviour
{
    public GameObject FirstSelected;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(FirstSelected);
    }
    
}
