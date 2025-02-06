using UnityEngine;
using FMODUnity;
using System.Collections;
public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance { get; private set; }

    //rayoSound
    [field: Header("Colectables")]
    [field: SerializeField] public EventReference rayoCollectedSound { get; private set; }
    //ZanahoriaSound
    [field: SerializeField] public EventReference zanahoriaCollectedSound { get; private set; }
    //Jump
    [field: Header("Player")]
    [field: SerializeField] public EventReference playerJump { get; private set; }
    [field: SerializeField] public EventReference playerLand { get; private set; }
    [field: SerializeField] public EventReference playerDeath { get; private set; }
    //ChargedJump
    [field: SerializeField] public EventReference playerChargedJump { get; private set; }
    //Music
    [field: Header("Music")]
    [field: SerializeField] public EventReference levelMusic { get; private set; }
    [field: SerializeField] public EventReference hubMusic { get; private set; }

    [field: Header("UI")]
    [field: SerializeField] public EventReference openPause { get; private set; }
    [field: SerializeField] public EventReference closePause { get; private set; }
    [field: SerializeField] public EventReference confirm { get; private set; }
    [field: SerializeField] public EventReference goBack { get; private set; }
    [field: SerializeField] public EventReference select { get; private set; }
    [field: SerializeField] public EventReference enterLevel { get; private set; }
    [field: Header("Props")]
    [field: SerializeField] public EventReference AcidLeak { get; private set; }
    [field: SerializeField] public EventReference AcidDrop { get; private set; }
    [field: SerializeField] public EventReference AcidGas { get; private set; }
    [field: SerializeField] public EventReference checkPoint { get; private set; }
    [field: SerializeField] public EventReference metalDoors { get; private set; }

    [field: Header("Enemies")]
    [field: SerializeField] public EventReference WalkingEnemy { get; private set; }
    [field: SerializeField] public EventReference FlyingEnemy { get; private set; }
    [field: SerializeField] public EventReference Shooter { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Más de 1 AudioManager en la escena");
        }
        instance = this;
    }
}
