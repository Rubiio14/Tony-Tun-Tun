using UnityEngine;

//Save system implementation from https://www.youtube.com/watch?v=uD7y4T4PVk0
public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance { get; private set; }
    public static bool IsSessionStarted;

    public SessionData SessionData;
    public SessionData DefaultData;

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

    public bool IsDataSavedInFile()
    {
        return FileManager.DoesSaveFileExists();
    }

    public void DeleteSaveData()
    {
        if (FileManager.DeleteSavefile())
        {
            IsSessionStarted = false;
            Debug.Log("Delete successful");
        }
    }

    public void SaveSessionDataToFile()
    {
        if (FileManager.WriteToSaveFile(SessionData.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public void LoadSessionDataFromFile()
    {
        if (FileManager.LoadFromSaveFile(out var json))
        {
            Debug.Log(json);

            Debug.Log("Load successful");
        }
    }

    public void ResetSessionData()
    {
        SessionData.CurrentLevelIndex = DefaultData.CurrentLevelIndex;
        for(int i = 0; i < SessionData.SessionLevels.Count; i++)
        {
            SessionData.SessionLevels[i].IsLocked = DefaultData.SessionLevels[i].IsLocked;
            for(int j = 0; j < SessionData.SessionLevels[i].SessionCarrots.Count; j++)
            {
                SessionData.SessionLevels[i].SessionCarrots[j].IsPicked = DefaultData.SessionLevels[i].SessionCarrots[j].IsPicked;
            }
        }
        
    }
}
