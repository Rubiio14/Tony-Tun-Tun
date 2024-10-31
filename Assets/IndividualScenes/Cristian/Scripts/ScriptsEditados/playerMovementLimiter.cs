using UnityEngine;

public class playerMovementLimiter : MonoBehaviour
{
    
    public static playerMovementLimiter instance;

    [SerializeField] bool _initialCharacterCanMove = true;
    public bool CharacterCanMove;

    private void OnEnable()
    {
        instance = this;
    }

    private void Start()
    {
        CharacterCanMove = _initialCharacterCanMove;
    }
}

