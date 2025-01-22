using UnityEngine;

public class LevelModelSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _lockedRepresentation;
    [SerializeField] private GameObject _unlockedRepresentation;
    public int Identifier;

    public void SwitchRepresentation(bool lockedState)
    {
        if(lockedState)
        {
            _lockedRepresentation.SetActive(true);
            _unlockedRepresentation.SetActive(false);
        }
        else
        {
            _lockedRepresentation.SetActive(false);
            _unlockedRepresentation.SetActive(true);
        }
    }

}
