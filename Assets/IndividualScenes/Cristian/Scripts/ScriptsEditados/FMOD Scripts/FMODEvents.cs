using UnityEngine;
using FMODUnity;
using System.Collections;
public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance { get; private set; }

    //rayoSound
    [field: Header("Rayo SFX")]
    [field: SerializeField] public EventReference rayoCollectedSound { get; private set; }

    //FootSteps
    [field: Header("FootSteps")]
    [field: SerializeField] public EventReference playerFootSteps { get; private set; }

    //Music
    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Más de 1 AudioManager en la escena");
        }
        instance = this;
    }
}
