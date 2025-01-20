using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayableLevel : Level
{
    //Will be used as an identifier for save/load functionality
    public int LevelIndex;
    public bool IsLocked;
    [field: SerializeField] public float LoadLevelDelay {  get; private set; }
    [field: SerializeField] public string SceneName { get; private set; }

    [Header("Carrot info")]
    [field: SerializeField] private int _carrotsToUnlock;
    public List<Carrot> CarrotsUnlocked;
    
    [SerializeField] private Vector3 _canvasPosition;

    [Header("Unlock")]
    [SerializeField] private GameObject _lockedLevelPrefab;
    [SerializeField] private GameObject _unlockedLevelPrefab;
    [SerializeField] private ParticleSystem _particleSystemUnlock;
    [SerializeField] private float _unlockTime;

    [Header("Unlock")]
    [SerializeField] private string _accessAnimation;

    public void Awake()
    {
        CarrotsUnlocked = new List<Carrot>();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HubManager.Instance.MarkAsPlatformArrival();
            if (IsLocked)
            {
                HubManager.Instance.ShowNumberOfCarrotsToUnlock(_carrotsToUnlock, _canvasPosition);
            }
            else
            {
                HubManager.Instance.ShowCurrentCarrotsInLevel(_canvasPosition);
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
        return SaveGameManager.Instance.TotalCarrots >= _carrotsToUnlock;
    }

    public void UnLock()
    {
        SaveGameManager.Instance.SaveLockedLevelStatus(false);
        HubManager.Instance.HideNumberOfCarrotsToUnlock();
        HubManager.Instance.ShowCurrentCarrotsInLevel( _canvasPosition);
        IsLocked = false; 
    }
}
