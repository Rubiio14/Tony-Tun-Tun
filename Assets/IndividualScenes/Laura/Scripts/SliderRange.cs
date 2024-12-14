using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
        }else if(eventData.moveDir == MoveDirection.Right)
        {
            _slider.value += _increment;
        }
    }

    override public void OnValueChanged()
    {
        Apply();
        //Sound or particle effect ?
    }

    override public void OnCancel(BaseEventData eventData)
    {
        Deactivate();
    }

    override public void OnSubmit(BaseEventData eventData)
    {
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
        AudioManager.Instance.ChangeMusicVolume(volume/_slider.maxValue);
    }

    public void LoadMusicFromSource()
    {
        _slider.value = AudioManager.Instance.GetCurrentMusicVolume() * _slider.maxValue;
        Apply();
    }

    public void LoadSFXFromSource()
    {
        _slider.value = AudioManager.Instance.GetCurrentSFXVolume() * _slider.maxValue;
        Apply();
    }

    public void ChangeSFXVolume(float volume)
    {
        AudioManager.Instance.ChangeSFXVolume(volume/_slider.maxValue);
    }
}
