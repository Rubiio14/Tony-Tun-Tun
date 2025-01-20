using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

//Save system implementation from https://www.youtube.com/watch?v=uD7y4T4PVk0
public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance { get; private set; }

    [SerializeField] private string _savegameFileName;
    public static SaveData SavedData { get; private set; }

    public DataBetweenScenes _currentSessionData;
    public static bool _firstTimeInHUB = true;

    public int CurrentLevelIndex { get { return _currentSessionData.CurrentLevelIndex; } set { _currentSessionData.CurrentLevelIndex = value; } }
    public int TotalCarrots { get { return _currentSessionData.TotalNumberOfCarrots; } set { _currentSessionData.TotalNumberOfCarrots = value; } }
    public List<Carrot> CarrotsUnlockedInLevel { get { return _currentSessionData.LevelInfo[_currentSessionData.CurrentLevelIndex].CarrotsUnlocked; } set { _currentSessionData.LevelInfo[_currentSessionData.CurrentLevelIndex].CarrotsUnlocked = value; } }

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

    private void Start()
    {
        //Check if file exist
        if (!IsDataSaved())
        {   //There is no saved data file
            if (_firstTimeInHUB && SceneManager.GetActiveScene().name == "HUB")
            {
                //First time you enter the HUB this session or you selected new game from main menu
                InitializeDataFromScene();
                _firstTimeInHUB = false;
            }
        }
        else
        {
            //There is a data file
            if (_firstTimeInHUB && SceneManager.GetActiveScene().name == "HUB")
            {
                //First time you enter the HUB this session
                LoadJsonData(_currentSessionData);
                _firstTimeInHUB = false;
            }
        }
        
        if(SceneManager.GetActiveScene().name == "Level0")
        {
            SceneManager.LoadScene("HUB");
        }
    }

    public bool IsDataSaved()
    {
        return FileManager.DoesFileExists(_savegameFileName);
    }

    public void DeleteSaveData()
    {
        if (FileManager.Delete(_savegameFileName))
        {
            _firstTimeInHUB = true;
            Debug.Log("Delete successful");
        }
    }

    public void SaveJsonData(ISaveable saveable)
    {
        SaveData savedData = new SaveData();
        
        saveable.PopulateSaveData(savedData);

        if (FileManager.WriteToFile(_savegameFileName, savedData.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public void LoadJsonData(ISaveable saveable)
    {
        if (FileManager.LoadFromFile(_savegameFileName, out var json))
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
        Level[] levels = GameObject.FindObjectsByType<Level>(FindObjectsSortMode.None);
        PlayableLevel playableLevel;
        _currentSessionData.Initialize();

        for (int i = 0; i < levels.Length; i++)
        {
            Debug.LogFormat("Level info: {0}", levels[i].name);
            playableLevel = levels[i] as PlayableLevel;
            if(playableLevel != null)
            {
                Debug.LogFormat("Playable info: {0}", levels[i].name);
                _currentSessionData.LevelInfo[playableLevel.LevelIndex] = playableLevel;
            }
        }
    }

    public PlayableLevel RetrieveLevelData(int levelIndex)
    {
        return _currentSessionData.LevelInfo[levelIndex];
    }

    public bool RetrieveLockedLevelStatus(int levelIndex)
    {
        return _currentSessionData.LevelInfo[levelIndex].IsLocked;
    }

    public void SaveLockedLevelStatus(bool isLocked)
    {
        _currentSessionData.LevelInfo[_currentSessionData.CurrentLevelIndex].IsLocked = isLocked;
    }

    public void SaveGame()
    {
        SaveJsonData(_currentSessionData);
    }

}
