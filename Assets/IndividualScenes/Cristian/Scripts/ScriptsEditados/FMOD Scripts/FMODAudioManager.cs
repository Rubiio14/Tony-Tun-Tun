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
        InitializeMusic(FMODEvents.instance.levelMusic);
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
        // Si el sonido no está en reproducción, iniciarlo
        if (!chargedJumpInstance.isValid())
        {
            chargedJumpInstance = CreateInstance(FMODEvents.instance.playerChargedJump);
            chargedJumpInstance.start();
            Debug.Log("Sonido de salto cargado iniciado.");
        }
    }

    public void StopChargedJump()
    {
        // Si el sonido está en reproducción, detenerlo y liberar la instancia
        if (chargedJumpInstance.isValid())
        {
            chargedJumpInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            chargedJumpInstance.release();
            chargedJumpInstance.clearHandle();
            Debug.Log("Sonido de salto cargado detenido.");
        }
    }

    private void Update()
    {
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
    }
}
