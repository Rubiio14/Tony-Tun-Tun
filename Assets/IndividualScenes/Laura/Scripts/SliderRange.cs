using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SliderRange : SelectLeftRight { 

    private Slider _slider;
    [SerializeField] private float _increment;
    [SerializeField] private TextMeshProUGUI _UISoundText;

    public void Awake()
    {
        //We assume this is going to be a prefab, configured specifically
        _slider = transform.GetChild(0).GetComponent<Slider>();
        _image = GetComponent<Image>();
    }

    public void OnEnable()
    {
        //Audio mixer, load initial values for _slider.value
        OnLoadedComponent.Invoke();
    }

    override public void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir == MoveDirection.Left)
        {
            _slider.value -= _increment;
        }
        else if(eventData.moveDir == MoveDirection.Right)
        {
            _slider.value += _increment;
        }
    }

    override public void OnValueChanged()
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.select);
        Apply();
    }

    override public void OnCancel(BaseEventData eventData)
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.goBack);
        Deactivate();
    }

    override public void OnSubmit(BaseEventData eventData)
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.goBack);

        Deactivate();
    }

    override public void Activate(GameObject previousSelected)
    {
        base.Activate(previousSelected);
        //If we want to do something else
    }

    override public void Deactivate()
    {
        base.Deactivate();
        //If we want to do something else
    }

    public void Apply()
    {
        _UISoundText.text = (_slider.value).ToString();
    }

    public void ChangeMusicVolume(float volume)
    {
        float musicVolume = volume / _slider.maxValue;
        SaveGameManager.Instance.SessionData.MusicVolume = musicVolume;
        FMODAudioManager.instance.MusicVolumeChange(musicVolume);
    }
    public void ChangeSFXVolume(float volume)
    {
        float sfxVolume = volume / _slider.maxValue;
        SaveGameManager.Instance.SessionData.SFXVolume = sfxVolume;
        FMODAudioManager.instance.SFXVolumeChange(sfxVolume);
    }

    public void LoadMusicFromSource()
    {
        _slider.value = SaveGameManager.Instance.SessionData.MusicVolume * _slider.maxValue;
        Apply();
    }

    public void LoadSFXFromSource()
    {
        _slider.value = SaveGameManager.Instance.SessionData.SFXVolume * _slider.maxValue;
        Apply();
    }

}
