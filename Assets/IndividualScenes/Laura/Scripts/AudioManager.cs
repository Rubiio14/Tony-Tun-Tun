using UnityEngine;
using UnityEngine.Localization.Settings;

//Implementation from https://www.daggerhartlab.com/unity-audio-and-sound-manager-singleton-script/
public class AudioManager : MonoBehaviour
{
    //TODO: Review after audio integration, this will probably change 
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource MusicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.clip = clip;
        SFXSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    // Play a random clip from an array, and randomize the pitch slightly.
    public void RandomSoundEffect(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);

        SFXSource.clip = clips[randomIndex];
        SFXSource.Play();
    }
    /*musicVolume must go from 0.0 to 1.0*/
    public void ChangeMusicVolume(float musicVolume)
    {
        MusicSource.volume = musicVolume;
    }
    /*sfxVolume must go from 0.0 to 1.0*/
    public void ChangeSFXVolume(float sfxVolume)
    {
        SFXSource.volume = sfxVolume;
    }
    /*musicVolume returns as a float from 0.0 to 1.0*/
    public float GetCurrentMusicVolume()
    {
        return MusicSource.volume;
    }
    /*sfxVolume returns as a float from 0.0 to 1.0*/
    public float GetCurrentSFXVolume()
    {
        return SFXSource.volume;
    }

    public void LoadValues()
    {
        //Load values from previous save file, load values on sources, slider will take this values from initialization
    }

    public void SaveValues()
    {
        //Save values in save file, will be values set when game saves last time
    }

}
