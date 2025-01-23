using UnityEngine;
using FMODUnity;
using FMOD.Studio;
public class FMODAudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float sfxVolume = 1;

    private Bus musicBus;
    private Bus sfxBus;
    public static FMODAudioManager instance { get; private set; }

    private EventInstance musicEventInstance;
    private EventInstance chargedJumpInstance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Más de 1 AudioManager en la escena");
        }
        instance = this;

        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.music);
    }
    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void SetMusicParameter(string parameterName, float parameterValue)
    {
        musicEventInstance.setParameterByName(parameterName, parameterValue);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }
    // Método para reproducir el sonido del salto cargado.
    public void PlayChargedJump()
    {
        if (chargedJumpInstance.isValid())
        {
            chargedJumpInstance.release();
        }
        chargedJumpInstance = CreateInstance(FMODEvents.instance.playerChargedJump);
        chargedJumpInstance.start();
        Invoke(nameof(StopChargedJump), 0.5f);
    }

    // Método para detener el sonido del salto cargado.
    public void StopChargedJump()
    {
        if (chargedJumpInstance.isValid())
        {
            chargedJumpInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            chargedJumpInstance.release();
            chargedJumpInstance.clearHandle();
            Debug.Log("Es Válido");
        }
        else
        {
            Debug.Log("No es Válido");
        }
    }

    private void Update()
    {
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
    }
}
