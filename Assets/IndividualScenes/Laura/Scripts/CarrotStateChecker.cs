using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CarrotStateChecker : MonoBehaviour
{
    [Tooltip("There must be 3 on the level")]
    [SerializeField] private List <Collectable> _collectables;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Collectable collectable in _collectables) {
            if(SaveGameManager.Instance.SessionData.SessionLevels[SaveGameManager.Instance.SessionData.CurrentLevelIndex].SessionCarrots[collectable.Index].IsPicked) {
                CollectablesManager.instance.IncrementCarrot();
                collectable.gameObject.SetActive(false);
            }
        }
    }

}
