using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.Rendering;
using System;

public class FMODAudioManager : MonoBehaviour
{
    private Bus musicBus;
    private Bus sfxBus;
    public static FMODAudioManager instance { get; private set; }

    private EventInstance musicEventInstance;
    private EventInstance chargedJumpInstance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }
    //Para probar nada mas Laura =), luego la playeas doden quieras
    /*public void Start()
    {
        InitializeMusic(FMODEvents.instance.levelMusic);
    }*/

    public void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void LoadVolumes()
    {
        MusicVolumeChange(SaveGameManager.Instance.SessionData.MusicVolume);
        SFXVolumeChange(SaveGameManager.Instance.SessionData.SFXVolume);
    }

    public void StopMusic()
    {
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void MusicVolumeChange(float volume)
    {
        musicBus.setVolume(volume);
    }

    public void SFXVolumeChange(float volume)
    {
        sfxBus.setVolume(volume);
    }

    public void SetMusicParameter(string parameterName, float parameterValue)
    {
        musicEventInstance.setParameterByName(parameterName, parameterValue);
    }
    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
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

    public void PlayChargedJump()
    {
        if (!chargedJumpInstance.isValid())
        {
            chargedJumpInstance = CreateInstance(FMODEvents.instance.playerChargedJump);
            chargedJumpInstance.start();
            Debug.Log("Sonido de salto cargado iniciado.");
        }
    }

    public void StopChargedJump()
    {
        if (chargedJumpInstance.isValid())
        {
            chargedJumpInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            chargedJumpInstance.release();
            chargedJumpInstance.clearHandle();
            Debug.Log("Sonido de salto cargado detenido.");
        }
    }
}
