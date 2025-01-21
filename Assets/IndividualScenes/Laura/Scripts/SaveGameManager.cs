using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

//Save system implementation from https://www.youtube.com/watch?v=uD7y4T4PVk0
public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance { get; private set; }
    public static SaveData SavedData { get; private set; }

    public static DataBetweenScenes CurrentSessionData;

    public SessionData SessionData;
    public SessionData AnotherSessionData;

    public int CurrentLevelIndex { get { return CurrentSessionData.CurrentLevelIndex; } set { CurrentSessionData.CurrentLevelIndex = value; } }
    public int TotalCarrots { get { return CurrentSessionData.TotalNumberOfCarrots; } set { CurrentSessionData.TotalNumberOfCarrots = value; } }


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


    public bool IsDataSaved()
    {
        return FileManager.DoesSaveFileExists();
    }

    public void DeleteSaveData()
    {
        if (FileManager.DeleteSavefile())
        {
            Debug.Log("Delete successful");
        }
    }

    public void SaveJsonData(ISaveable saveable)
    {
        SaveData savedData = new SaveData();
        
        saveable.PopulateSaveData(savedData);

        if (FileManager.WriteToSaveFile(savedData.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public void LoadJsonData(ISaveable saveable)
    {
        if (FileManager.LoadFromSaveFile(out var json))
        {
            SavedData = new SaveData();

            SavedData.LoadFromJson(json);

            saveable.LoadFromSaveData(SavedData);

            Debug.Log("Load complete");
        }
    }
    
    public void InitializeDataFromScene()
    {
        //Look in scene for PLayableLevel scripts
        List<Level> levels = GameObject.FindObjectsByType<Level>(FindObjectsSortMode.None).OrderBy(level => level.LevelIndex).ToList();
        CurrentSessionData = ScriptableObject.CreateInstance<DataBetweenScenes>();
        CurrentSessionData.Initialize();
        CurrentSessionData.LevelInfo = levels;
    }

    /*public PlayableLevel RetrieveLevelData(int levelIndex)
    {
        return CurrentSessionData.LevelInfo[levelIndex];
    }*/

    /*public bool RetrieveLockedLevelStatus(int levelIndex)
    {
        return CurrentSessionData.LevelInfo[levelIndex].IsLocked;
    }*/

    /*public void SaveLockedLevelStatus(bool isLocked)
    {
        CurrentSessionData.LevelInfo[CurrentSessionData.CurrentLevelIndex].IsLocked = isLocked;
    }*/

    public void SaveGame()
    {
        SaveJsonData(CurrentSessionData);
    }

}
