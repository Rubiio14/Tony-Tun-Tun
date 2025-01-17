using System;
using System.Collections.Generic;
using UnityEngine;

//Save system implementation from https://www.youtube.com/watch?v=uD7y4T4PVk0
public class GameManager : MonoBehaviour 
{
    public static GameManager Instance { get; private set; }

    public SaveData SavedData { get; private set; }
    [SerializeField] private string _savegameFileName;

    private Waypoint _currentPlayingLevel;

    //Carrots
    [field:SerializeField] public int TotalCarrots { get; private set; }

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
    }

    public void SaveJsonData(IEnumerable<ISaveable> saveables)
    {
        SaveData saveData = new SaveData();
        foreach (var saveable in saveables)
        {
            saveable.PopulateSaveData(saveData);
        }

        if (FileManager.WriteToFile(_savegameFileName, saveData.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public void LoadJsonData(IEnumerable<ISaveable> saveables)
    {
        if (FileManager.LoadFromFile(_savegameFileName, out var json))
        {
            SaveData saveData = new SaveData();
            saveData.LoadFromJson(json);

            foreach (var saveable in saveables)
            {
                saveable.LoadFromSaveData(saveData);
            }

            Debug.Log("Load complete");
        }
    }


}
