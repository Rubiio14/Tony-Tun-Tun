using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayableLevel : Waypoint, ISaveable
{
    //Will be used as an identifier for save/load functionality
    [SerializeField] private int _levelIndex;
    [field: SerializeField] public bool IsLocked { get; private set; }
    [field: SerializeField] public float LoadLevelDelay {  get; private set; }
    [field: SerializeField] public string SceneName { get; private set; }

    [Header("Carrot info")]
    [field: SerializeField] private int _carrotsToUnlock;
    [field: SerializeField] public Carrot[] CarrotsUnlocked { get; private set; }
    //level Carrot Canvas

    [Header("Unlock")]
    [SerializeField] private GameObject _lockedLevelPrefab;
    [SerializeField] private GameObject _unlockedLevelPrefab;
    [SerializeField] private ParticleSystem _particleSystemUnlock;
    [SerializeField] private float _unlockTime;

    [Header("Unlock")]
    [SerializeField] private string _accessAnimation;

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HubManager.Instance.MarkAsPlatformArrival();
            if (IsLocked)
            {
                HubManager.Instance.ShowNumberOfCarrotsToUnlock(_carrotsToUnlock);
            }
            else
            {
                HubManager.Instance.ShowCurrentCarrotsInLevel(CarrotsUnlocked);
            }
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HubManager.Instance.MarkAsPlatformDeparture();
            if (IsLocked)
            {
                HubManager.Instance.HideNumberOfCarrotsToUnlock();
            }
            else
            {
                HubManager.Instance.HideCurrentCarrotsInLevel();
            }
        }
    }

    public bool CanBeUnlocked()
    {
        return GameManager.Instance.TotalCarrots >= _carrotsToUnlock;
    }

    public void PopulateSaveData(SaveData saveData)
    {
        SaveData.LevelData levelData = new SaveData.LevelData();
        levelData.Index = _levelIndex;
        levelData.IsLocked = IsLocked;
        levelData.CarrotList = new List<SaveData.LevelData.Carrot>();

        //levelData.CarrotList.Add();
    }

    public void LoadFromSaveData(SaveData saveData)
    {

    }
}
